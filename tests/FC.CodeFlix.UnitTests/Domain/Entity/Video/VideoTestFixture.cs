
using FC.CodeFlix.Catalog.Tests.Shared;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;
namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Video;

public class VideoTestFixture
{
    private readonly VideoDataGenerator _dataGenerator = new();
    private readonly CategoryDataGenerator _categoryDataGenerator = new();
    private readonly GenreDataGenerator _genreDataGenerator = new();
    private readonly CastMemberDataGenerator _castMemberDataGenerator = new();

    public CategoryDataGenerator CategoryDataGenerator => _categoryDataGenerator;

    public GenreDataGenerator GenreDataGenerator => _genreDataGenerator;

    public CastMemberDataGenerator CastMemberDataGenerator => _castMemberDataGenerator;

    public DomainEntity.Video GetValidVideo()
        => _dataGenerator.GetValidVideo();
}

[CollectionDefinition(nameof(VideoTestFixture))]
public class VideoTestFixtureCollection
    : ICollectionFixture<VideoTestFixture>
{ }