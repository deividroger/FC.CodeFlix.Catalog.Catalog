using Elasticsearch.Net;
using FC.Codeflix.Catalog.E2ETests;
using FC.CodeFlix.Catalog.E2ETests.Base;
using FC.CodeFlix.Catalog.Infra.ES;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.CodeFlix.Catalog.E2ETests.Graphql.Categories.Common;

public class CategoryTestFixture : IDisposable
{
    public CustomerWebApplicationFactory<Program> WebAppFactory { get; private set; } = null!;

    public CatalogClient GraphQLClient { get; private set; } = null!;

    public CategoryTestFixture()
    {
        WebAppFactory = new CustomerWebApplicationFactory<Program>();

        _ = WebAppFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri(WebAppFactory.BaseUrl)
        });

        GraphQLClient = WebAppFactory.Services.GetRequiredService<CatalogClient>();
        CreateCategoryIndexAsync().GetAwaiter().GetResult();
    }

    private async Task CreateCategoryIndexAsync()
    {
        var esClient = WebAppFactory.Services.GetRequiredService<IElasticClient>();

        var response = await esClient.Indices.CreateAsync(ElasticsearchIndices.Category, c => c
            .Map<CategoryModel>(m => m
                .Properties(ps => ps
                    .Keyword(t => t
                        .Name(category => category.Id)
                    )
                    .Text(t => t
                        .Name(category => category.Name)
                        .Fields(fs => fs
                            .Keyword(k => k
                                .Name(category => category.Name.Suffix("keyword")))
                        )
                    )
                    .Text(t => t
                        .Name(category => category.Description)
                    )
                    .Boolean(b => b
                        .Name(category => category.IsActive)
                    )
                    .Date(d => d
                        .Name(category => category.CreatedAt)
                    )
                )
            )
        );
    }

    public void DeleteAll()
    {
        var elasticClient = WebAppFactory.Services.GetRequiredService<IElasticClient>();
        elasticClient.DeleteByQuery<CategoryModel>(del =>
            del.Query(
                q => q.QueryString(qs => qs.Query("*"))
            ).Conflicts(Conflicts.Proceed)
        );
    }

    public void Dispose()
    {
        var elasticClient = WebAppFactory.Services.GetRequiredService<IElasticClient>();

        elasticClient.Indices.Delete(ElasticsearchIndices.Category);
    }
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection : ICollectionFixture<CategoryTestFixture>
{ }
