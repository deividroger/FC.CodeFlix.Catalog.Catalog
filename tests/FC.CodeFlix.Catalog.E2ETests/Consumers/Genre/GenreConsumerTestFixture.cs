using FC.CodeFlix.Catalog.Api.Genres;
using FC.CodeFlix.Catalog.E2ETests.Base.Fixture;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using FC.CodeFlix.Catalog.Infra.Messaging.Configuration;
using FC.CodeFlix.Catalog.Infra.Messaging.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
namespace FC.CodeFlix.Catalog.E2ETests.Consumers.Genre;

public class GenreConsumerTestFixture : GenreTestFixtureBase
{
    public readonly KafkaConfiguration _kafkaConfiguration;

    public GenreConsumerTestFixture() : base()
    {
        _kafkaConfiguration = WebAppFactory.Services.GetRequiredService<IOptions<KafkaConfiguration>>().Value;

        //Wait a bit for the consumer get assigned a partition, mainly in case of rebalancing
        Thread.Sleep(15_000);
    }

    public Task PublishMessageAsync<T>(MessageModel<T> message)
        where T : GenrePayloadModel
        => PublishMessageAsync(
            typeof(T) == typeof(GenreCategoryPayloadModel) ? _kafkaConfiguration.GenreCategoryConsumer : 
            _kafkaConfiguration.GenreConsumer,
            message);

    public MessageModel<T> BuildValidMessage<T>(string operation, GenreModel genreModel)
        where T : GenrePayloadModel
    {
        var message = new MessageModel<T>()
        {
            Payload = new MessageModelPayload<T>
            {
                Op = operation,
            }
        };

        dynamic genrePayload;
        if (typeof(T) == typeof(GenreCategoryPayloadModel))
        {
             genrePayload = new GenreCategoryPayloadModel
            {
                Id = genreModel.Id,
            };
        }
        else
        {
             genrePayload = new GenrePayloadModel
            {
                Id = genreModel.Id,
            };
        }

        if (operation == "d")
        {
            message.Payload.Before = genrePayload;
        }
        else
        {
            message.Payload.After = genrePayload;
        }

        return message;
    }

    public MessageModel<T> BuildValidMessage<T>(string operation)
        where T : GenrePayloadModel 
        => BuildValidMessage<T>(operation, DataGenerator.GetGenreModelList(1)[0]);

    internal Domain.Entity.Genre GetValidGenre(Guid id)
        => DataGenerator.GetValidGenre(id);
}

[CollectionDefinition(nameof(GenreConsumerTestFixture))]
public class GenreConsumerTestFixtureCollection
    : ICollectionFixture<GenreConsumerTestFixture>
{ }