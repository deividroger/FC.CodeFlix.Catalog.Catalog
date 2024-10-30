using FC.CodeFlix.Catalog.Domain.Repositories;
using FC.CodeFlix.Catalog.Tests.Shared;
using NSubstitute;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;

namespace FC.CodeFlix.Catalog.UnitTests.Application.UseCases.Categories.Common;

public class CategoryUseCaseFixture 
{
    public CategoryDataGenerator DataGenerator { get;  }

    public CategoryUseCaseFixture()
        => DataGenerator = new CategoryDataGenerator();


    public ICategoryRepository GetMockRepository()
        => Substitute.For<ICategoryRepository>();

    public DomainEntity.Category GetValidCategory()
        => DataGenerator.GetValidCategory();

}

[CollectionDefinition(nameof(CategoryUseCaseFixture))]
public class CategoryUseCaseFixtureCollection : ICollectionFixture<CategoryUseCaseFixture> { }