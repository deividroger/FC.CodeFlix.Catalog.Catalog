using FC.CodeFlix.Catalog.Domain.Repositories.DTOs;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using FC.CodeFlix.Catalog.Tests.Shared;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.CodeFlix.Catalog.E2ETests.Base.Fixture;

public class GenreTestFixtureBase
{
    public CustomerWebApplicationFactory<Program> WebAppFactory { get; private set; } = null!;


    public IElasticClient ElasticClient { get; private set; }

    public GenreDataGenerator DataGenerator { get; private set; }

    protected GenreTestFixtureBase()
    {
        DataGenerator = new GenreDataGenerator();

        WebAppFactory = new CustomerWebApplicationFactory<Program>();

        _ = WebAppFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri(WebAppFactory.BaseUrl)
        });

        ElasticClient = WebAppFactory.Services.GetRequiredService<IElasticClient>();

        ElasticClient.CreateGenreIndexAsync().GetAwaiter().GetResult();
    }

    public IList<GenreModel> GetGenreModelList(int count = 10)
        => DataGenerator.GetGenreModelList(count);

    public void DeleteAll()
        => ElasticClient.DeleteDocuments<GenreModel>();

    public void Dispose()
        => ElasticClient.DeleteGenreIndex();
}
