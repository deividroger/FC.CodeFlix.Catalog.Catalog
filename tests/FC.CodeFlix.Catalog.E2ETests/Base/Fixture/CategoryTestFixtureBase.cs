using FC.CodeFlix.Catalog.Infra.ES.Models;
using FC.CodeFlix.Catalog.Tests.Shared;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.CodeFlix.Catalog.E2ETests.Base.Fixture;

public class CategoryTestFixtureBase: FixtureBase, IDisposable
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

        ElasticClient.CreateCategoryIndexAsync().GetAwaiter().GetResult();
    }

    public IList<CategoryModel> GetCategoryModelList(int count = 10)
        => DataGenerator.GetCategoryModelList(count);

    public void DeleteAll()
        => ElasticClient.DeleteDocuments<CategoryModel>();

    public void Dispose()
        => ElasticClient.DeleteCategoryIndex();
}
