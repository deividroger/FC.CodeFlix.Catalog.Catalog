using FC.CodeFlix.Catalog.Application.UseCases.Category.SaveCategory;
using FC.CodeFlix.Catalog.UnitTests.Application.UseCases.Categories.Common;

namespace FC.CodeFlix.Catalog.UnitTests.Application.UseCases.Categories.SaveCategory;

public class SaveCategoryTestFixture : CategoryUseCaseFixture
{
    public SaveCategoryInput GetValidInput()
        => new SaveCategoryInput(
            Guid.NewGuid(), 
            DataGenerator.GetValidCategoryName(), 
            DataGenerator.GetValidCategoryDescription(), 
            DateTime.Now, DataGenerator.GetRandomBoolean());

    public SaveCategoryInput GetInValidInput()
        => new SaveCategoryInput(
            Guid.NewGuid(),
            null, 
            DataGenerator.GetValidCategoryDescription(),
            DateTime.Now, 
            DataGenerator.GetRandomBoolean());
}

[CollectionDefinition(nameof(SaveCategoryTestFixture))]
public class SaveCategoryTestFixtureCollection : ICollectionFixture<SaveCategoryTestFixture> { }