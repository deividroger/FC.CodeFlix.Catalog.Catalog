using FC.Codeflix.Catalog.E2ETests;
using FC.CodeFlix.Catalog.E2ETests.Base.Fixture;

namespace FC.CodeFlix.Catalog.E2ETests.Graphql.Categories.Common;

public class CategoryTestFixture : CategoryTestFixtureBase, IDisposable
{

    public CatalogClient GraphQLClient { get; }

    public CategoryTestFixture() : base()
        => GraphQLClient = WebAppFactory.GraphqlClient;

}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection : ICollectionFixture<CategoryTestFixture>
{ }
