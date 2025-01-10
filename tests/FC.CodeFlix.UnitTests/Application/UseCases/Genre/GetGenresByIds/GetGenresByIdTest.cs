using FC.CodeFlix.Catalog.UnitTests.Application.UseCases.Genre.Common;
using FluentAssertions;
using NSubstitute;
using UseCase = FC.CodeFlix.Catalog.Application.UseCases.Genre.GetGenresByIds;

namespace FC.CodeFlix.Catalog.UnitTests.Application.UseCases.Genre.GetGenresByIds;

[Collection(nameof(GenreUseCaseTestFixture))]
public class GetGenresByIdTest
{
    private readonly GenreUseCaseTestFixture _fixture;

    public GetGenresByIdTest(GenreUseCaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Trait("Application", "[UseCase] SaveGenre")]
    [Fact(DisplayName = nameof(GetGenresByIds_WhenReceive_AValid_Input_Returns_Genres))]
    public async Task GetGenresByIds_WhenReceive_AValid_Input_Returns_Genres()
    {
        var repository = _fixture.GetMockRepository();

        var genres = _fixture.GetGenreList();

        var expectedOutput = genres.Select(genre => new
        {
            genre.Id,
            genre.Name,
            genre.CreatedAt,
            genre.IsActive,
            Categories = genre.Categories.Select(category => new { category.Id, category.Name })
        });
        
        var ids = expectedOutput.Select(x=>x.Id).ToList();

        repository.GetGenresByIdsAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(genres.AsEnumerable()));

        var useCase = new UseCase.GetGenresByIds(repository);

        var input = new UseCase.GetGenresByIdsInput(ids);

        var output = await useCase.Handle(input, CancellationToken.None);


        output.Should().BeEquivalentTo(expectedOutput);

        await repository.Received(1).GetGenresByIdsAsync(ids, Arg.Any<CancellationToken>());
    }
}
