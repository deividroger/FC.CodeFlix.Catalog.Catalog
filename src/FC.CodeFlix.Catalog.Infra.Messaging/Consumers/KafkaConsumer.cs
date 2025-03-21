﻿using Confluent.Kafka;
using Confluent.Kafka.Admin;
using FC.CodeFlix.Catalog.Domain.Exceptions;
using FC.CodeFlix.Catalog.Infra.Messaging.Common;
using FC.CodeFlix.Catalog.Infra.Messaging.Configuration;
using FC.CodeFlix.Catalog.Infra.Messaging.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FC.CodeFlix.Catalog.Infra.Messaging.Consumers;

public class KafkaConsumer<TMessage> : BackgroundService
    where TMessage : class
{
    private readonly KafkaConsumerConfiguration _configuration;
    private readonly ILogger<KafkaConsumer<TMessage>> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IProducer<string, string> _producer;

    private readonly List<(Func<MessageModel<TMessage>, bool> Predicate, Type Type)> _messageHandlers = new();

    public KafkaConsumer(KafkaConsumerConfiguration configuration, ILogger<KafkaConsumer<TMessage>> logger, IServiceProvider serviceProvider)
    {
        _configuration = configuration;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _producer = new ProducerBuilder<string, string>(GetProducerConfig()).Build();
    }

    private ProducerConfig GetProducerConfig()
       => new()
       {
           BootstrapServers = _configuration.BootstrapServers,
           Acks = Acks.All

       };

    private ConsumerConfig GetConsumerConfig()
        => new()
        {
            BootstrapServers = _configuration.BootstrapServers,
            GroupId = _configuration.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true,
            EnableAutoOffsetStore = false,
        };

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await EnsureTopicsAreCreatedAsync();
        var config = GetConsumerConfig();
        var topic = _configuration.Topic;

        using var consumer = new ConsumerBuilder<string, string>(config).Build();

        consumer.Subscribe(topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            var consumerResult = await Task.Run(() => consumer.Consume((int)TimeSpan.FromSeconds(30).TotalMilliseconds), stoppingToken);

            if (consumerResult == null || consumerResult.IsPartitionEOF || stoppingToken.IsCancellationRequested)
            {
                continue;
            }

            if (_configuration.ConsumeDelaySeconds > 0)
            {
                await Task.Delay(_configuration.ConsumeDelaySeconds * 1_000, stoppingToken);
            }

            await HandleMessageAsync(consumerResult.Message, stoppingToken);
            consumer.StoreOffset(consumerResult);

        }
        consumer.Close();
    }

    private async Task HandleMessageAsync(Message<string, string> message, CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateAsyncScope();

            var messageModel = JsonSerializer.Deserialize<MessageModel<TMessage>>(message.Value, SerializerConfiguration.JsonSerializerOptions);

            var messageHandlerType = _messageHandlers
                .FirstOrDefault(tuple => tuple.Predicate.Invoke(messageModel!))
                .Type;

            if (messageHandlerType is null)
            {
                _logger.LogError("None of the message handlers predicates was satisfied by the message {@message}", message);
                return;
            }

            var handler = (IMessageHandler<TMessage>?)scope.ServiceProvider.GetRequiredService(messageHandlerType);

            if (handler is null)
            {
                _logger.LogError("None message handle found of type: {@messageHandlerType}", messageHandlerType);
                return;
            }

            await handler.HandleMessageAsync(messageModel!, cancellationToken);
        }
        catch (Exception ex)
        {
            await OnErrorAsync(ex, message, cancellationToken);
        }
    }
    private async Task OnErrorAsync(Exception exception, Message<string, string> message, CancellationToken cancellationToken)
    {
        if (exception is JsonException or BusinessRuleException)
        {
            _logger.LogError(exception, "Non-retryable Error. Sending to DLQ. {error}", exception.Message);
            await _producer.ProduceAsync(_configuration.DlqTopic, message, cancellationToken);
            return;
        }

        var topic = _configuration.HasRetry ? _configuration.RetryTopic! : _configuration.DlqTopic!;

        _logger.LogError(exception, "Retryable error. Sending to {topic}. Error: {error}", topic, exception.Message);

        await _producer.ProduceAsync(topic, message, cancellationToken);
    }

    public void AddMessageHandler<T>(Func<MessageModel<TMessage>, bool> predicate)
    {
        _messageHandlers.Add((predicate, typeof(T)));
    }

    public void AddMessageHandler(Type type, Func<MessageModel<TMessage>, bool> predicate)
    {
        _messageHandlers.Add((predicate, type));
    }

    public async Task EnsureTopicsAreCreatedAsync()
    {
        var kafkaAdminConfig = new AdminClientConfig()
        {
            BootstrapServers = _configuration.BootstrapServers
        };

        var topics = new[] { _configuration.Topic, _configuration.DlqTopic };

        using var KafkaAdmin = new AdminClientBuilder(kafkaAdminConfig).Build();

        try
        {
            var topicSpecification = topics.Select(topicName => new TopicSpecification
            {
                Name = topicName,
                ReplicationFactor = 1, //do not use hardcode
                NumPartitions = 1 //do not use hardcode
            });
            await KafkaAdmin.CreateTopicsAsync(topicSpecification);
        }
        catch (CreateTopicsException ex)
        when (ex.Message.Contains("already exists"))
        {

            _logger.LogWarning("Topic already exists: {error}", ex.ToString());
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _producer.Dispose();
        return base.StopAsync(cancellationToken);
    }
}
