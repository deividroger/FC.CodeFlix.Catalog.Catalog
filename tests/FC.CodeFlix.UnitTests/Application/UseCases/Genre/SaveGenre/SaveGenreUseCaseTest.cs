using FC.CodeFlix.Catalog.Domain.Exceptions;
using FluentAssertions;
using NSubstitute;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;
using UseCase = FC.CodeFlix.Catalog.Application.UseCases.Genre.SaveGenre;
namespace FC.CodeFlix.Catalog.UnitTests.Application.UseCases.Genre.SaveGenre;

[Collection(nameof(SaveGenreUseCaseTestFixture))]
public class SaveGenreUseCaseTest
{
    private readonly SaveGenreUseCaseTestFixture _fixture;

    public SaveGenreUseCaseTest(SaveGenreUseCaseTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(SaveValidGenre))]
    [Trait("Application", "[UseCase] SaveGenre")]
    public async Task SaveValidGenre()
    {
        var repository = _fixture.GetMockRepository();
        var useCase = new UseCase.SaveGenre(repository);
        var input = _fixture.GetValidInput();

        var output = await useCase.Handle(input,CancellationToken.None);

        await repository.Received(1).SaveAsync(Arg.Any<DomainEntity.Genre>(),Arg.Any<CancellationToken>());

        output.Should().NotBeNull();
        output.Id.Should().Be(input.Id);  
        output.Name.Should().Be(input.Name);
        output.CreatedAt.Should().Be(input.CreatedAt);
        output.IsActive.Should().Be(input.IsActive);

        output.Categories.Should().BeEquivalentTo(input.Categories);

    }

    [Fact(DisplayName = nameof(SaveInValidGenre))]
    [Trait("Application", "[UseCase] SaveGenre")]
    public async Task SaveInValidGenre()
    {
        var repository = _fixture.GetMockRepository();
        var useCase = new UseCase.SaveGenre(repository);
        var input = _fixture.GetInvalidInput();

       var action = async () => await useCase.Handle(input,CancellationToken.None);


        await action.Should().ThrowAsync<EntityValidationException>().WithMessage("Name should not be empty or null");


        await repository.DidNotReceive().SaveAsync(Arg.Any<DomainEntity.Genre>(), Arg.Any<CancellationToken>());
    }

}
