using FC.Codeflix.Catalog.E2ETests;
using FC.CodeFlix.Catalog.E2ETests.Graphql.Categories.Common;

namespace FC.CodeFlix.Catalog.E2ETests.Graphql.Categories.SaveCategory;


public class SaveCategoryTestFixture : CategoryTestFixture
{
    public SaveCategoryTestFixture() : base()
    {

    }

    public SaveCategoryInput GetValidInput()
        => new SaveCategoryInput
        {
            Id = Guid.NewGuid(),
            Name = DataGenerator.GetValidCategoryName(),
            Description = DataGenerator.GetValidCategoryDescription(),
            CreatedAt = DateTime.UtcNow.Date,
            IsActive = DataGenerator.GetRandomBoolean(),
        };

    public SaveCategoryInput GetInValidInput()
        => new SaveCategoryInput
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            Description = DataGenerator.GetValidCategoryDescription(),
            CreatedAt = DateTime.UtcNow.Date,
            IsActive = DataGenerator.GetRandomBoolean(),
        };
}

[CollectionDefinition(nameof(SaveCategoryTestFixture))]
public class SaveCategoryTestFixtureCollection : ICollectionFixture<SaveCategoryTestFixture>
{ }
