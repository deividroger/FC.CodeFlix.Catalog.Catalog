using FC.Codeflix.Catalog.E2ETests;
using FC.CodeFlix.Catalog.E2ETests.Base.Fixture;

namespace FC.CodeFlix.Catalog.E2ETests.Graphql.Video.Common;

public class VideoTestFixture : VideoTestFixtureBase
{
    public CatalogClient GraphQLClient { get; }

    public VideoTestFixture()
        => GraphQLClient = WebAppFactory.GraphqlClient;
}

[CollectionDefinition(nameof(VideoTestFixture))]
public class VideoTestFixtureFixtureCollection : ICollectionFixture<VideoTestFixture>
{

}