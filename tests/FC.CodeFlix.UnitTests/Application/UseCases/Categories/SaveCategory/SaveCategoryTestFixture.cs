using FC.CodeFlix.Catalog.Application.UseCases.Category.SaveCategory;
using FC.CodeFlix.UnitTests.Application.UseCases.Categories.Common;

namespace FC.CodeFlix.UnitTests.Application.UseCases.Categories.SaveCategory;

public class SaveCategoryTestFixture: CategoryUseCaseFixture
{
    public SaveCategoryInput GetValidInput()
        => new SaveCategoryInput(Guid.NewGuid(), GetValidName(), GetValidDescription(), DateTime.Now, GetRandomBoolean());

    public SaveCategoryInput GetInValidInput()
        => new SaveCategoryInput(Guid.NewGuid(), null, GetValidDescription(), DateTime.Now, GetRandomBoolean());
}

[CollectionDefinition(nameof(SaveCategoryTestFixture))]
public class SaveCategoryTestFixtureCollection: ICollectionFixture<SaveCategoryTestFixture> { }