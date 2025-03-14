using FC.CodeFlix.Catalog.E2ETests.Base.Fixture;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using FC.CodeFlix.Catalog.Infra.Messaging.Configuration;
using FC.CodeFlix.Catalog.Infra.Messaging.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FC.CodeFlix.Catalog.E2ETests.Consumers.CastMember;

public class CastMemberConsumerTestFixture : CastMemberTestFixtureBase
{
    public readonly KafkaConfiguration _kafkaConfiguration;

    public CastMemberConsumerTestFixture() : base()
    {
        _kafkaConfiguration = WebAppFactory.Services.GetRequiredService<IOptions<KafkaConfiguration>>().Value;

        //Wait a bit for the consumer get assigned a partition, mainly in case of rebalancing
        Thread.Sleep(15_000);
    }

    public Task PublishMessageAsync(object message)
        => PublishMessageAsync(_kafkaConfiguration.CastMemberConsumer, message);

    public MessageModel<CastMemberPayloadModel> BuildValidMessage(string operation, CastMemberModel castMemberModel)
    {
        var message = new MessageModel<CastMemberPayloadModel>()
        {
            Payload = new MessageModelPayload<CastMemberPayloadModel>
            {
                Op = operation,
            }
        };

        var categoryPayload = new CastMemberPayloadModel
        {
            Id = castMemberModel.Id,
            Name = castMemberModel.Name,
            CreatedAt = castMemberModel.CreatedAt,
            Type = castMemberModel.Type,
            
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

    public MessageModel<CastMemberPayloadModel> BuildValidMessage(string operation)
        => BuildValidMessage(operation, DataGenerator.GetCastMemberModelList(1)[0]);
}

[CollectionDefinition(nameof(CastMemberConsumerTestFixture))]
public class CastMemberConsumerTestFixtureCollection
    : ICollectionFixture<CastMemberConsumerTestFixture>
{ }