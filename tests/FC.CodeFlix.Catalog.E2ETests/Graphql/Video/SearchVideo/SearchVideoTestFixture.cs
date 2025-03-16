using FC.CodeFlix.Catalog.E2ETests.Graphql.Video.Common;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using RepositoryDTOs = FC.CodeFlix.Catalog.Domain.Repositories.DTOs;

namespace FC.CodeFlix.Catalog.E2ETests.Graphql.Video.SearchVideo;

public class SearchVideoTestFixture : VideoTestFixture
{
    public IList<VideoModel> GetVideoModelList(List<string> names)
        => DataGenerator.GetVideoModelList(names);
    public IList<VideoModel> CloneVideosListOrdered(List<VideoModel> videoList, string orderBy, RepositoryDTOs.SearchOrder direction)
        => DataGenerator.CloneVideosListOrdered(videoList, orderBy, direction);
}

[CollectionDefinition(nameof(SearchVideoTestFixture))]
public class SearchVideoTestFixtureCollection : ICollectionFixture<SearchVideoTestFixture>
{

}