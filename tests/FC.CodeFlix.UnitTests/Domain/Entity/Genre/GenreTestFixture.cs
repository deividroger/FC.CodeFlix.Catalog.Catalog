using FC.CodeFlix.Catalog.Tests.Shared;

namespace FC.CodeFlix.Catalog.UnitTests.Domain.Entity.Genre;

public class GenreTestFixture
{
    public readonly GenreDataGenerator _dataGenerator = new();

    public Catalog.Domain.Entity.Genre GetValidGenre()
        => _dataGenerator.GetValidGenre();

}

[CollectionDefinition(nameof(GenreTestFixture))]
public class GenreTestFixtureCollection : ICollectionFixture<GenreTestFixture>
{

}