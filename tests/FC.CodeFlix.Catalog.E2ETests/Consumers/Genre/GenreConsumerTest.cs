using FC.CodeFlix.Catalog.Infra.ES.Models;
using FC.CodeFlix.Catalog.Infra.HttpClients.Configuration;
using FC.CodeFlix.Catalog.Infra.HttpClients.Models;
using FC.CodeFlix.Catalog.Infra.Messaging.Models;
using FluentAssertions;
using Nest;
using System.Text.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace FC.CodeFlix.Catalog.E2ETests.Consumers.Genre;

[Collection(nameof(GenreConsumerTestFixture))]
public class GenreConsumerTest : IDisposable
{
    private readonly GenreConsumerTestFixture _fixture;
    private readonly WireMockServer wireMockServer = WireMockServer.Start(5555);

    public GenreConsumerTest(GenreConsumerTestFixture fixture)
    {
        _fixture = fixture;
        _fixture.ConfigureGetTokenMock(wireMockServer);
    }

    [Theory(DisplayName = nameof(GenreEvent_WhenOperationIsCreateOrRead_SavesGenre))]
    [Trait("E2E/Consumer", "Genre")]
    [InlineData("c",nameof(GenrePayloadModel))]
    [InlineData("r", nameof(GenrePayloadModel))]
    [InlineData("c", nameof(GenreCategoryPayloadModel))]
    [InlineData("r", nameof(GenreCategoryPayloadModel))]
    public async Task GenreEvent_WhenOperationIsCreateOrRead_SavesGenre(string operation, string messageType)
    {
        dynamic message = messageType == nameof(GenreCategoryPayloadModel ) ? 
            _fixture.BuildValidMessage<GenreCategoryPayloadModel>(operation) : 
            _fixture.BuildValidMessage<GenrePayloadModel>(operation);

        Domain.Entity.Genre genre = _fixture.GetValidGenre(message.Payload.After.Id);

        var apiResponse = new DataWrapper<Domain.Entity.Genre>(genre);

        var apiReponseBody = JsonSerializer.Serialize(apiResponse, SerializerConfiguration.SnakeCaseSerializerOptions);

        var adminCatalogRequest = Request.Create()
            .WithPath($"/genres/{genre.Id}")
            .WithHeader("Authorization", "Bearer *")
            .UsingGet();

        var adminCatalogResponse = Response.Create()
            .WithStatusCode(200)
            .WithHeader("Content-Type", "application/json")
            .WithBody(apiReponseBody);

        wireMockServer.Given(adminCatalogRequest)
            .RespondWith(adminCatalogResponse);

        await _fixture.PublishMessageAsync(message);
        await Task.Delay(8_000);

        var persisted = await _fixture.ElasticClient.GetAsync<GenreModel>(genre.Id);

        persisted.Found.Should().BeTrue();

        var document = persisted.Source;
        document.Should().NotBeNull();

        document.Id.Should().Be(genre.Id);
        document.Name.Should().Be(genre.Name);
        document.CreatedAt.Date.Should().Be(genre.CreatedAt.Date);
        document.IsActive.Should().Be(genre.IsActive);
        document.Categories.Should().BeEquivalentTo(genre.Categories.Select(c => new { c.Id, c.Name }).ToList());

        wireMockServer.FindLogEntries(adminCatalogRequest).Should().HaveCount(1);
        wireMockServer.FindLogEntries(_fixture.AuthRequestBuilderMock)
            .Should()
            .HaveCountLessThanOrEqualTo(1);

    }

    [Fact(DisplayName = nameof(GenreEvent_WhenOperationIsUpdate_SavesGenre))]
    [Trait("E2E/Consumer", "Genre")]

    public async Task GenreEvent_WhenOperationIsUpdate_SavesGenre()
    {
        var examplesList = _fixture.GetGenreModelList();


        var example = examplesList[2];

        await _fixture.ElasticClient.IndexManyAsync(examplesList);

        var message = _fixture.BuildValidMessage<GenrePayloadModel>("u", example);

        var genre = _fixture.GetValidGenre(message.Payload.After.Id);

        var apiResponse = new DataWrapper<Domain.Entity.Genre>(genre);

        var apiReponseBody = JsonSerializer.Serialize(apiResponse, SerializerConfiguration.SnakeCaseSerializerOptions);

        var adminCatalogRequest = Request.Create()
                 .WithPath($"/genres/{genre.Id}")
                 .WithHeader("Authorization", "Bearer access_token.jwt")
                 .UsingGet();

        var adminCatalogResponse = Response.Create()
            .WithStatusCode(200)
            .WithHeader("Content-Type", "application/json")
            .WithBody(apiReponseBody);

        wireMockServer.Given(adminCatalogRequest)
            .RespondWith(adminCatalogResponse);


        await _fixture.PublishMessageAsync(message);
        await Task.Delay(8_000);

        var persisted = await _fixture.ElasticClient.GetAsync<GenreModel>(genre.Id);

        persisted.Found.Should().BeTrue();

        var document = persisted.Source;
        document.Should().NotBeNull();

        document.Id.Should().Be(genre.Id);
        document.Name.Should().Be(genre.Name);
        document.CreatedAt.Date.Should().Be(genre.CreatedAt.Date);
        document.IsActive.Should().Be(genre.IsActive);
        document.Categories.Should().BeEquivalentTo(genre.Categories.Select(c => new { c.Id, c.Name }).ToList());

        wireMockServer.FindLogEntries(adminCatalogRequest).Should().HaveCount(1);
        wireMockServer.FindLogEntries(_fixture.AuthRequestBuilderMock)
            .Should()
            .HaveCountLessThanOrEqualTo(1);
    }

    [Fact(DisplayName = nameof(GenreEvent_WhenOperationIsDelete_DeleteGenre))]
    [Trait("E2E/Consumer", "Genre")]
    public async Task GenreEvent_WhenOperationIsDelete_DeleteGenre()
    {
        var examplesList = _fixture.GetGenreModelList();

        var example = examplesList[2];

        await _fixture.ElasticClient.IndexManyAsync(examplesList);

        var message = _fixture.BuildValidMessage<GenrePayloadModel>("d", example);

        var genre = message.Payload.Before;

        await _fixture.PublishMessageAsync(message);
        await Task.Delay(8_000);

        var persisted = await _fixture.ElasticClient.GetAsync<GenreModel>(genre.Id);

        persisted.Found.Should().BeFalse();
    }

    public void Dispose()
    {
        wireMockServer.Dispose();
        _fixture.DeleteAll();
    }
}
