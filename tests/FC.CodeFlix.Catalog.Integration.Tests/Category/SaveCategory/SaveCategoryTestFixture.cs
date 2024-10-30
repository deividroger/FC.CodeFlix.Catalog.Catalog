using FC.CodeFlix.Catalog.Application.UseCases.Category.SaveCategory;
using FC.CodeFlix.Catalog.Integration.Tests.Category.Common;

namespace FC.CodeFlix.Catalog.Integration.Tests.Category.SaveCategory;

public class SaveCategoryTestFixture: CategoryTestFixure
{
    public SaveCategoryInput GetValidInput()
        => new SaveCategoryInput(Guid.NewGuid(), GetValidCategoryName(), GetValidCategoryDescription(), DateTime.Now, GetRandomBoolean());

    public SaveCategoryInput GetInValidInput()
        => new SaveCategoryInput(Guid.NewGuid(), null, GetValidCategoryDescription(), DateTime.Now, GetRandomBoolean());
}

[CollectionDefinition(nameof(SaveCategoryTestFixture))]
public class SaveCategoryTestFixtureCollection : ICollectionFixture<SaveCategoryTestFixture> { }
