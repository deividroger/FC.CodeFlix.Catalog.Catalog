using FC.CodeFlix.Catalog.Domain.Exceptions;
using FluentAssertions;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;
namespace FC.CodeFlix.Catalog.UnitTests.Domain.Entity.Category;

[Collection(nameof(CategoryTestFixure))]
public class CategoryTest
{
    private readonly CategoryTestFixure _categoryTestFixure;

    public CategoryTest(CategoryTestFixure categoryTestFixure)
        => _categoryTestFixure = categoryTestFixure;

    [Fact(DisplayName = nameof(Instanciate))]
    [Trait("Domain", "Category - Agregates")]
    public void Instanciate()
    {
        var validCategory = _categoryTestFixure.GetValidCategory();

        var category = new DomainEntity.Category(validCategory.Id, validCategory.Name, validCategory.Description,validCategory.CreatedAt);

        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().Be(validCategory.Id);
        category.CreatedAt.Should().Be(validCategory.CreatedAt);
        category.IsActive.Should().BeTrue();


    }


    [Theory(DisplayName = nameof(InstanciateWithIsActive))]
    [Trait("Domain", "Category - Agregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstanciateWithIsActive(bool isActive)
    {
        var validCategory = _categoryTestFixure.GetValidCategory();

        var category = new DomainEntity.Category(validCategory.Id, validCategory.Name, validCategory.Description, validCategory.CreatedAt,isActive);

        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().Be(validCategory.Id);
        category.CreatedAt.Should().Be(validCategory.CreatedAt);
        (category.IsActive).Should().Be(isActive);

    }

    [Fact(DisplayName = nameof(InstanciateErrorWhenIdIEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstanciateErrorWhenIdIEmpty()
    {
        var validCategory = _categoryTestFixure.GetValidCategory();
        Action action =
            () => new DomainEntity.Category(Guid.Empty, validCategory.Name, validCategory.Description, validCategory.CreatedAt);

        action.Should().Throw<EntityValidationException>().WithMessage("Id should not be empty or null");

    }

    [Theory(DisplayName = nameof(InstanciateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void InstanciateErrorWhenNameIsEmpty(string? name)
    {
        var validCategory = _categoryTestFixure.GetValidCategory();

        Action action =
            () => new  DomainEntity.Category(validCategory.Id, name!, validCategory.Description, validCategory.CreatedAt);

        action.Should().Throw<EntityValidationException>().WithMessage("Name should not be empty or null");

    }

    [Fact(DisplayName = nameof(InstanciateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstanciateErrorWhenDescriptionIsNull()
    {
        var validCategory = _categoryTestFixure.GetValidCategory();
        Action action =
            () => new DomainEntity.Category(validCategory.Id, validCategory.Name, null!, validCategory.CreatedAt);

        action.Should().Throw<EntityValidationException>().WithMessage("Description should not be null");

    }
}