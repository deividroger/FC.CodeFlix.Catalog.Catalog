using FC.CodeFlix.Catalog.Application.UseCases.Genre.SaveGenre;
using FC.CodeFlix.Catalog.Integration.Tests.Genre.Common;

namespace FC.CodeFlix.Catalog.Integration.Tests.Genre.SaveGenre;

public class SaveGenreTestFixture: GenreTestFixture
{

    public SaveGenreTestFixture() : base()
    {
        
    }
    public SaveGenreInput GetValidInput()
        => DataGenerator.GetValidSaveGenreInput();

    public SaveGenreInput GetInvalidInput()
        => DataGenerator.GetInvalidSaveGenreInput();
}


[CollectionDefinition(nameof(SaveGenreTestFixture))]
public class GenreTestFixtureCollection : ICollectionFixture<SaveGenreTestFixture>
{ }