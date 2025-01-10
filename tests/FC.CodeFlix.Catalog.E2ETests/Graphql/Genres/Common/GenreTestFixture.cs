using FC.Codeflix.Catalog.E2ETests;
using FC.CodeFlix.Catalog.E2ETests.Base.Fixture;
using Microsoft.Extensions.DependencyInjection;

namespace FC.CodeFlix.Catalog.E2ETests.Graphql.Genres.Common;

public class GenreTestFixture: GenreTestFixtureBase
{
    public CatalogClient GraphQLClient { get; }

    public GenreTestFixture()
    {
        GraphQLClient = WebAppFactory.Services.GetRequiredService<CatalogClient>();
    }
}

[CollectionDefinition(nameof(GenreTestFixture))]
public class GenreTestFixtureCollection: ICollectionFixture<GenreTestFixture> { 

}