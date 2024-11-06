using FC.Codeflix.Catalog.E2ETests;
using FC.CodeFlix.Catalog.E2ETests.Base.Fixture;
using Microsoft.Extensions.DependencyInjection;

namespace FC.CodeFlix.Catalog.E2ETests.Graphql.Categories.Common;

public class CategoryTestFixture : CategoryTestFixtureBase, IDisposable
{


    public CatalogClient GraphQLClient { get; }


    public CategoryTestFixture() : base()
        => GraphQLClient = WebAppFactory.Services.GetRequiredService<CatalogClient>();

}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection : ICollectionFixture<CategoryTestFixture>
{ }
