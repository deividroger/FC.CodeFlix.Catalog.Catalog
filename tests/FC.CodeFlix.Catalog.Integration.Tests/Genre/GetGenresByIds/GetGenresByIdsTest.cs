using FC.CodeFlix.Catalog.Application.UseCases.Genre.GetGenresByIds;
using FC.CodeFlix.Catalog.Infra.ES;
using FC.CodeFlix.Catalog.Integration.Tests.Genre.Common;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.CodeFlix.Catalog.Integration.Tests.Genre.GetGenresByIds;

[Collection(nameof(GenreTestFixture))]    
public class GetGenresByIdsTest : IDisposable
{
    private readonly GenreTestFixture _fixture;

    public GetGenresByIdsTest(GenreTestFixture fixture)
    {
        this._fixture = fixture;
    }

    [Fact(DisplayName = nameof(GetGenresByIds_WhenReceive_AValid_Input_Returns_Genres))]
    [Trait("Integration", "[UseCase] GetGenresByIds")]
    public async Task GetGenresByIds_WhenReceive_AValid_Input_Returns_Genres()
    {
        var elasticClient = _fixture.ElasticClient;

        var genres = _fixture.GetGenreModelList();

        await elasticClient.IndexManyAsync(genres);
        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.Genre);

        var expectedOutput = new[]
        {
            genres[3],
            genres[5]
        };

        var ids = expectedOutput.Select(x => x.Id).ToList();

        var serviceProvider = _fixture.ServiceProvider;

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var input = new GetGenresByIdsInput(ids);

        var output = await mediator.Send(input);
           

        output.Should().BeEquivalentTo(expectedOutput);

    }

    public void Dispose()
    {
        _fixture.DeleteAll();
    }
}
