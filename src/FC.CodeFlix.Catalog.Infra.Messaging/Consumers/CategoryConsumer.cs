using Confluent.Kafka;
using FC.CodeFlix.Catalog.Domain.Exceptions;
using FC.CodeFlix.Catalog.Infra.Messaging.Configuration;
using FC.CodeFlix.Catalog.Infra.Messaging.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace FC.CodeFlix.Catalog.Infra.Messaging.Consumers;

public class CategoryConsumer : BackgroundService
{
    private readonly KafkaConfiguration _configuration;
    private readonly ILogger<CategoryConsumer> _logger;
    private readonly IServiceProvider _serviceProvider;

    public CategoryConsumer(IOptions<KafkaConfiguration> configuration, ILogger<CategoryConsumer> logger, IServiceProvider serviceProvider)
    {
        _configuration = configuration.Value;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    private ConsumerConfig GetConsumerConfig()
        => new()
        {
            BootstrapServers = _configuration.BoostrapServers,
            GroupId = _configuration.CategoryConsumer.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true,
            EnableAutoOffsetStore = false,
        };

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = GetConsumerConfig();
        var topic = _configuration.CategoryConsumer.Topic;

        using var consumer = new ConsumerBuilder<string, string>(config).Build();

        consumer.Subscribe(topic);
        while (!stoppingToken.IsCancellationRequested)
        {
            var consumerResult = await Task.Run(() => consumer.Consume((int)TimeSpan.FromSeconds(30).TotalMilliseconds), stoppingToken);

            if (consumerResult == null || consumerResult.IsPartitionEOF || stoppingToken.IsCancellationRequested)
            {
                continue;
            }

            await HandleMessageAsync(consumerResult.Message, stoppingToken);
            consumer.StoreOffset(consumerResult);

        }
        consumer.Close();
    }

    private async Task HandleMessageAsync(Message<string, string> message, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateAsyncScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var messageModel = JsonSerializer.Deserialize<MessageModel<CategoryPayloadModel>>(message.Value, SerializerConfiguration.JsonSerializerOptions);

        switch (messageModel!.Payload.Operation)
        {
            case MessageModelOperation.Create:
            case MessageModelOperation.Read:
            case MessageModelOperation.Update:
                var saveInput = messageModel.Payload.After.ToSaveCategoryInput();
                await mediator.Send(saveInput, cancellationToken);
                break;
            case MessageModelOperation.Delete:
                try
                {
                    var deleteInput = messageModel.Payload.Before.ToDeleteCategoryInput();
                    await mediator.Send(deleteInput, cancellationToken);
                }
                catch (NotFoundException)
                {
                    _logger.LogError("Category not found. Message: {message}", message.Value);
                }
                break;
            default:
                _logger.LogError("Invalid operation: `{operation}", messageModel.Payload.Op);
                break;
        }
    }
}
