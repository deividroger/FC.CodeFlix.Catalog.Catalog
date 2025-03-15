using FC.CodeFlix.Catalog.Application.UseCases.Video.SaveVideo;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using FC.CodeFlix.Catalog.Integration.Tests.Video.Common;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FC.CodeFlix.Catalog.Integration.Tests.Video.SaveVideo;

[Collection(nameof(VideoTestFixture))]
public class SaveVideoTest : IDisposable
{
    private readonly VideoTestFixture _fixture;

    public SaveVideoTest(VideoTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(SaveVideo_WhenInputIsValid_PersistsVideo))]
    [Trait("Integration", "[UseCase] SaveVideo")]
    public async Task SaveVideo_WhenInputIsValid_PersistsVideo()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = _fixture.ElasticClient;
        var input = new SaveVideoInput(Guid.NewGuid());

        var output = await mediator.Send(input);

        var persisted = await elasticClient
            .GetAsync<VideoModel>(input.Id);
        persisted.Found.Should().BeTrue();
        var document = persisted.Source;
        document.Should().NotBeNull();
        document.Id.Should().Be(input.Id);
        output.Should().NotBeNull();
        output.Should().BeEquivalentTo(document);
    }

    public void Dispose() => _fixture.DeleteAll();
}
