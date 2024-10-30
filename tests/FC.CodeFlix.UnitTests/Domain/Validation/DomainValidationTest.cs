using Bogus;
using FC.CodeFlix.Catalog.Domain.Exceptions;
using FC.CodeFlix.Catalog.Domain.Validation;
using FluentAssertions;

namespace FC.CodeFlix.Catalog.UnitTests.Domain.Validation;

public class DomainValidationTest
{
    private Faker Faker { get; set; } = new Faker();

    [Fact(DisplayName = nameof(NotNullOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOk()
    {
        var value = Faker.Commerce.ProductName();

        Action action = () => DomainValidation.NotNull(value, "value");

        action.Should().NotThrow();

    }

    [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullThrowWhenNull()
    {
        string? value = null;

        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action = () => DomainValidation.NotNull(value, fieldName);

        action.Should().Throw<EntityValidationException>().WithMessage($"{fieldName} should not be null");

    }

    [Theory(DisplayName = nameof(NotNullOrEmpyThrowWhenEmpy))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void NotNullOrEmpyThrowWhenEmpy(string? target)
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action = () => DomainValidation.NotNullOrEmpty(target, fieldName);

        action.Should().Throw<EntityValidationException>().WithMessage($"{fieldName} should not be empty or null");
    }


    [Theory(DisplayName = nameof(GuidNotNullOrEmptyThrowWhenNullOrEmpty))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("")]
    [InlineData(null)]
    public void GuidNotNullOrEmptyThrowWhenNullOrEmpty(string? target)
    {
        Guid? value = target == null ? (Guid?)null : Guid.Empty;
        string fieldName = "Id";

        Action action = () => DomainValidation.NotNullOrEmpty(value, fieldName);

        action.Should().Throw<EntityValidationException>().WithMessage($"{fieldName} should not be empty or null");
    }

    [Fact(DisplayName = nameof(NotNullOrEmpy))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOrEmpy()
    {
        var target = Faker.Commerce.ProductName();

        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action = () => DomainValidation.NotNullOrEmpty(target, fieldName);

        action.Should().NotThrow();
    }

}