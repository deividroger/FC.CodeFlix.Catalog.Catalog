using Confluent.Kafka;
using FC.CodeFlix.Catalog.E2ETests.Base.Fixture;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using FC.CodeFlix.Catalog.Infra.Messaging.Configuration;
using FC.CodeFlix.Catalog.Infra.Messaging.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace FC.CodeFlix.Catalog.E2ETests.Consumers.Category;

public class CategoryConsumerTestFixture : CategoryTestFixtureBase
{
    public readonly KafkaConfiguration _kafkaConfiguration;

    public CategoryConsumerTestFixture():base()
    {
        _kafkaConfiguration = WebAppFactory.Services.GetRequiredService<IOptions<KafkaConfiguration>>().Value;

        //Wait a bit for the consumer get assigned a partition, mainly in case of rebalancing
        Thread.Sleep(15_000);
    }

    public async Task PublishMessageAsync(object message)
    {
        var config = new ProducerConfig { BootstrapServers = _kafkaConfiguration.CategoryConsumer.BoostrapServers };

        using var producer = new ProducerBuilder<string, string>(config).Build();

        var rawMessage = JsonSerializer.Serialize(message, SerializerConfiguration.JsonSerializerOptions);

        _ = await producer.ProduceAsync(
                          _kafkaConfiguration.CategoryConsumer.Topic,
                          new Message<string, string>
                          {
                              Key = Guid.NewGuid().ToString(),
                              Value = rawMessage
                          });
    }

    public MessageModel<CategoryPayloadModel> BuildValidMessage(string operation, CategoryModel categoryModel)
    {
        var message = new MessageModel<CategoryPayloadModel>()
        {
            Payload = new MessageModelPayload<CategoryPayloadModel>
            {
                Op = operation,
            }
        };

        var categoryPayload = new CategoryPayloadModel
        {
            Id = categoryModel.Id,
            Name = categoryModel.Name,
            CreatedAt = categoryModel.CreatedAt,
            Description = categoryModel.Description,
            IsActive = categoryModel.IsActive,
        };

        if (operation == "d")
        {
            message.Payload.Before = categoryPayload;
        }
        else
        {
            message.Payload.After = categoryPayload;
        }

        return message;
    }

    public MessageModel<CategoryPayloadModel> BuildValidMessage(string operation)
        => BuildValidMessage(operation, DataGenerator.GetCategoryModelList(1)[0]);
}


[CollectionDefinition(nameof(CategoryConsumerTestFixture))]
public class CategoryConsumerTestFixtureCollection
    : ICollectionFixture<CategoryConsumerTestFixture>
{ }