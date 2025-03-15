using FC.CodeFlix.Catalog.Domain.Gateways;
using FC.CodeFlix.Catalog.Tests.Shared;

namespace FC.CodeFlix.Catalog.Integration.Tests.Fakes;

public class AdminCatalogFake : IAdminCatalogGateway
{
    private readonly GenreDataGenerator _genreDataGenerator = new();
    private readonly VideoDataGenerator _videoDataGenerator = new();
    
    private readonly CategoryDataGenerator _categoryDataGenerator = new();
    private readonly CastMemberDataGenerator _castMemberDataGenerator = new();

    public Task<Domain.Entity.Genre> GetGenreAsync(Guid id, CancellationToken cancellationToken)
        => Task.FromResult(_genreDataGenerator.GetValidGenre(id));

    public Task<Domain.Entity.Video> GetVideoAsync(Guid id, CancellationToken cancellationToken)
    {
        var video = _videoDataGenerator.GetValidVideo(id);

        video.AddGenres(_genreDataGenerator.GetValidGenre());
        video.AddCategories(_categoryDataGenerator.GetValidCategory());
        video.AddCastMembers(_castMemberDataGenerator.GetValidCastMember());

        return Task.FromResult(video);

    }
}