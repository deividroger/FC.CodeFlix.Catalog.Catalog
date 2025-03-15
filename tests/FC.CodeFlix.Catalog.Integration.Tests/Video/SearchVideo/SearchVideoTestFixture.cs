using FC.CodeFlix.Catalog.Infra.ES.Models;
using FC.CodeFlix.Catalog.Integration.Tests.Video.Common;
using FC.CodeFlix.Catalog.Domain.Repositories.DTOs;
namespace FC.CodeFlix.Catalog.Integration.Tests.Video.SearchVideo;

public class SearchVideoTestFixture : VideoTestFixture
{
    public IList<VideoModel> GetVideoModelList(IEnumerable<string> titles)
        => DataGenerator.GetVideoModelList(titles);

    public IList<VideoModel> CloneVideosListOrdered(List<VideoModel> examples, string orderBy, SearchOrder inputOrder)
        => DataGenerator.CloneVideosListOrdered(examples, orderBy, inputOrder);
}

[CollectionDefinition(nameof(SearchVideoTestFixture))]
public class SearchVideoTestFixtureCollection
    : ICollectionFixture<SearchVideoTestFixture>
{ }
