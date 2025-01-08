using FC.CodeFlix.Catalog.Application.UseCases.Genre.DeleteGenre;
using FC.CodeFlix.Catalog.Domain.Exceptions;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using FC.CodeFlix.Catalog.Integration.Tests.Genre.Common;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.CodeFlix.Catalog.Integration.Tests.Genre.DeleteGenre;


[Collection(nameof(GenreTestFixture))]
public class DeleteGenreTest: IDisposable
{
    private readonly GenreTestFixture _fixture;

    public DeleteGenreTest(GenreTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteGenreWhenReceivesAndExistingId_DeletesGenre))]
    [Trait("Integration", "[UseCase] DeleteGenre")]
    public async Task DeleteGenreWhenReceivesAndExistingId_DeletesGenre()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var elasticClient = _fixture.ElasticClient;

        var genreExamples = _fixture.GetGenreModelList();

        await elasticClient.IndexManyAsync(genreExamples);

        var input = new DeleteGenreInput(genreExamples[3].Id);

        await mediator.Send(input, CancellationToken.None);

        var deletedGenre = await elasticClient.GetAsync<GenreModel>(input.Id);

        deletedGenre.Found.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(DeleteGenreWhenReceivesAndNonExistingId_ThrowsException))]
    [Trait("Integration", "[UseCase] DeleteGenre")]
    public async Task DeleteGenreWhenReceivesAndNonExistingId_ThrowsException()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = _fixture.ElasticClient;

        var categoriesExample = _fixture.GetGenreModelList();

        await elasticClient.IndexManyAsync(categoriesExample);

        var input = new DeleteGenreInput(Guid.NewGuid());

        var action = async () => await mediator.Send(input, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>()
                    .WithMessage($"Genre '{input.Id}' not found.");
    }

    public void Dispose()
        => _fixture.DeleteAll();
}
