using FC.CodeFlix.Catalog.Domain.Gateways;
using FC.CodeFlix.Catalog.Domain.Repositories;
using FC.CodeFlix.Catalog.Tests.Shared;
using NSubstitute;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;

namespace FC.CodeFlix.Catalog.UnitTests.Application.UseCases.Video.Common;

public class VideoUseCaseTestFixture
{
    public VideoDataGenerator DataGenerator { get; }

    public VideoUseCaseTestFixture()
        => DataGenerator = new VideoDataGenerator();

    public IVideoRepository GetMockRepository()
        => Substitute.For<IVideoRepository>();

    public IAdminCatalogGateway GetMockAdminCatalogGateway()
        => Substitute.For<IAdminCatalogGateway>();

    public DomainEntity.Video GetValidVideo()
        => DataGenerator.GetValidVideo();

    public IList<DomainEntity.Video> GetVideoList(int count = 10)
        => DataGenerator.GetVideoList(count);
}

[CollectionDefinition(nameof(VideoUseCaseTestFixture))]
public class VideoUseCaseFixtureCollection : ICollectionFixture<VideoUseCaseTestFixture> { }
