using FC.CodeFlix.Catalog.Domain.Repositories;
using FC.CodeFlix.Catalog.UnitTests.Common;
using NSubstitute;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;

namespace FC.CodeFlix.UnitTests.Application.UseCases.Categories.Common;

public class CategoryUseCaseFixture: BaseFixture
{
    public ICategoryRepository GetMockRepository() 
        => Substitute.For<ICategoryRepository>();

    public string GetValidName()
        => Faker.Commerce.Categories(1)[0];

    public string GetValidDescription()
        => Faker.Commerce.ProductDescription();

    public DomainEntity.Category GetValidCategory()
        => new(Guid.NewGuid(), GetValidName(), GetValidDescription(), DateTime.Now, GetRandomBoolean());

}

[CollectionDefinition(nameof(CategoryUseCaseFixture))]
public class CategoryUseCaseFixtureCollection : ICollectionFixture<CategoryUseCaseFixture> { }