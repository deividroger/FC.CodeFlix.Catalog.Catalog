using FC.CodeFlix.Catalog.UnitTests.Application.UseCases.Genre.Common;
using NSubstitute;
using UseCase = FC.CodeFlix.Catalog.Application.UseCases.Genre.DeleteGenre;

namespace FC.CodeFlix.Catalog.UnitTests.Application.UseCases.Genre.DeleteGenre;

[Collection(nameof(GenreUseCaseTestFixture))]
public class DeleteGenreUseCaseTest
{
    private readonly GenreUseCaseTestFixture _fixture;

    public DeleteGenreUseCaseTest(GenreUseCaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Trait("Application", "[UseCase] SaveGenre")]
    [Fact(DisplayName = nameof(DeleteGenre))]
    public async Task DeleteGenre()
    {
        var repository = _fixture.GetMockRepository();

        var useCase = new UseCase.DeleteGenre(repository);

        var input = new UseCase.DeleteGenreInput(Guid.NewGuid());

        await useCase.Handle(input,CancellationToken.None);

        await repository.Received(1).DeleteAsync(input.Id,Arg.Any<CancellationToken>());

    }
}