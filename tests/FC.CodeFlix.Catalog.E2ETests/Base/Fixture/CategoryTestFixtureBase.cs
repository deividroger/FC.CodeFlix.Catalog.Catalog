using FC.CodeFlix.Catalog.Infra.ES.Models;
using FC.CodeFlix.Catalog.Tests.Shared;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.CodeFlix.Catalog.E2ETests.Base.Fixture;

public class CategoryTestFixtureBase
{
    public CustomerWebApplicationFactory<Program> WebAppFactory { get; private set; } = null!;


    public IElasticClient ElasticClient { get; private set; }

    public CategoryDataGenerator DataGenerator { get; private set; }

    protected CategoryTestFixtureBase()
    {
        DataGenerator = new CategoryDataGenerator();

        WebAppFactory = new CustomerWebApplicationFactory<Program>();

        _ = WebAppFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri(WebAppFactory.BaseUrl)
        });

        ElasticClient = WebAppFactory.Services.GetRequiredService<IElasticClient>();

        ElasticSearchOperations.CreateCategoryIndexAsync(ElasticClient).GetAwaiter().GetResult();
    }

    public IList<CategoryModel> GetCategoryModelList(int count = 10)
        => DataGenerator.GetCategoryModelList(count);

    public void DeleteAll()
        => ElasticSearchOperations.DeleteCategoryDocuments(ElasticClient);

    public void Dispose()
        => ElasticSearchOperations.DeleteCategoryIndex(ElasticClient);
}
