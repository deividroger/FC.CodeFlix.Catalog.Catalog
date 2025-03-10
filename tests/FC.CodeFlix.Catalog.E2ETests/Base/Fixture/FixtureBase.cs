using Confluent.Kafka;
using FC.CodeFlix.Catalog.Infra.HttpClients.Models;
using FC.CodeFlix.Catalog.Infra.Messaging.Configuration;
using System.Text.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace FC.CodeFlix.Catalog.E2ETests.Base.Fixture;

public class FixtureBase
{
    private readonly IRequestBuilder _authRequestBuilderMock = Request.Create()
            .WithPath("*/protocol/openid-connect/token")
            .UsingPost();
    protected async Task PublishMessageAsync(KafkaConsumerConfiguration configuration, object message)
    {
        var config = new ProducerConfig { BootstrapServers = configuration.BootstrapServers };

        using var producer = new ProducerBuilder<string, string>(config).Build();

        var rawMessage = JsonSerializer.Serialize(message, SerializerConfiguration.JsonSerializerOptions);

        _ = await producer.ProduceAsync(
                          configuration.Topic,
                          new Message<string, string>
                          {
                              Key = Guid.NewGuid().ToString(),
                              Value = rawMessage
                          });
    }

    public IRequestBuilder AuthRequestBuilderMock => _authRequestBuilderMock;

    public void ConfigureGetTokenMock(WireMockServer mockServer)
    {
        var authResponse = new AuthenticationResponseModel()
        {
            AccessToken = "access_token.jwt",
            ExpiresInSeconds = 300
        };

        var authResponseBody = JsonSerializer.Serialize(authResponse, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        mockServer.Given(_authRequestBuilderMock)
            .RespondWith(Response.Create()
            .WithStatusCode(200)
            .WithBody(authResponseBody));
    }
}
