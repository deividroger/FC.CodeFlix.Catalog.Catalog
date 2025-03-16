using FC.CodeFlix.Catalog.Infra.ES.Models;
using FC.CodeFlix.Catalog.Tests.Shared;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.CodeFlix.Catalog.E2ETests.Base.Fixture;

public class VideoTestFixtureBase: FixtureBase, IDisposable
{
    public CustomerWebApplicationFactory<Program> WebAppFactory { get; private set; } = null!;


    public IElasticClient ElasticClient { get; private set; }

    public VideoDataGenerator DataGenerator { get; private set; }

    protected VideoTestFixtureBase()
    {
        DataGenerator = new VideoDataGenerator();

        WebAppFactory = new CustomerWebApplicationFactory<Program>();

        _ = WebAppFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri(WebAppFactory.BaseUrl)
        });

        ElasticClient = WebAppFactory.Services.GetRequiredService<IElasticClient>();

        ElasticClient.CreateVideoIndexAsync().GetAwaiter().GetResult();
    }

    public IList<VideoModel> GetVideoModelList(int count = 10)
        => DataGenerator.GetVideoModelList(count);

    public void DeleteAll()
        => ElasticClient.DeleteDocuments<VideoModel>();

    public void Dispose()
        => ElasticClient.DeleteVideoIndex();
}
