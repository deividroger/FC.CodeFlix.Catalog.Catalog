
using FC.CodeFlix.Catalog.Domain.Exceptions;
using FluentAssertions;
using NSubstitute;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;
using UseCase = FC.CodeFlix.Catalog.Application.UseCases.Category.SaveCategory;
namespace FC.CodeFlix.UnitTests.Application.UseCases.Categories.SaveCategory;


[Collection(nameof(SaveCategoryTestFixture))]
public class SaveCategoryTest
{
    private readonly SaveCategoryTestFixture _fixture;

    public SaveCategoryTest(SaveCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName =nameof(SaveValidCategory))]
    [Trait("Application", "[UseCase] SaveCategory")]
    public async Task SaveValidCategory()
    {
        var repository = _fixture.GetMockRepository();

        var useCase = new UseCase.SaveCategory(repository);

        var input = _fixture.GetValidInput();

        var output = await useCase.Handle(input,CancellationToken.None);

        await repository.Received(1).SaveAsync(
            Arg.Any<DomainEntity.Category>(), Arg.Any<CancellationToken>());

        output.Should().NotBeNull();
        output.Id.Should().Be(input.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.CreatedAt.Should().Be(input.CreatedAt);
        output.IsActive.Should().Be(input.IsActive);
    }

    [Fact(DisplayName = nameof(SaveInValidCategory))]
    [Trait("Application", "[UseCase] SaveCategory")]
    public async Task SaveInValidCategory()
    {
        var repository = _fixture.GetMockRepository();

        var useCase = new UseCase.SaveCategory(repository);

        var input = _fixture.GetInValidInput();

        var action  = async () => await useCase.Handle(input, CancellationToken.None);

       await repository.DidNotReceive().SaveAsync(
            Arg.Any<DomainEntity.Category>(), Arg.Any<CancellationToken>());

        await action.Should().ThrowAsync<EntityValidationException>().WithMessage("Name should not be empty or null");
    }
}