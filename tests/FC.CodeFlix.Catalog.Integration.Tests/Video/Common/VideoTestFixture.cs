using FC.CodeFlix.Catalog.Infra.ES.Models;
using FC.CodeFlix.Catalog.Integration.Tests.Common;
using FC.CodeFlix.Catalog.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.CodeFlix.Catalog.Integration.Tests.Video.Common;

public class VideoTestFixture: BaseFixture
{
    public IElasticClient ElasticClient { get; }

    public VideoDataGenerator DataGenerator { get; }

    public VideoTestFixture()
    {
        ElasticClient = ServiceProvider.GetRequiredService<IElasticClient>();

        DataGenerator = new VideoDataGenerator();

        ElasticClient.CreateVideoIndexAsync().GetAwaiter().GetResult();

    }

    public List<VideoModel> GetVideoModelList(int count = 10)
        => DataGenerator.GetVideoModelList(count);

    public void DeleteAll()
        => ElasticClient.DeleteDocuments<VideoModel>();

    public void Dispose()
        => ElasticClient.DeleteGenreIndex();
}

[CollectionDefinition(nameof(VideoTestFixture))]
public class VideoTestFixtureCollection : ICollectionFixture<VideoTestFixture>
{ }