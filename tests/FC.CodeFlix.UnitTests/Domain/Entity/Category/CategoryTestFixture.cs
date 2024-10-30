using FC.CodeFlix.Catalog.Tests.Shared;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;
namespace FC.CodeFlix.Catalog.UnitTests.Domain.Entity.Category;

public class CategoryTestFixure
{

    public CategoryDataGenerator DataGenerator { get; }    

    public CategoryTestFixure() 
        => DataGenerator = new CategoryDataGenerator();


    public DomainEntity.Category GetValidCategory()
        => DataGenerator.GetValidCategory();
}

[CollectionDefinition(nameof(CategoryTestFixure))]
public class CategoryTestFixtureCollection : ICollectionFixture<CategoryTestFixure>
{ }