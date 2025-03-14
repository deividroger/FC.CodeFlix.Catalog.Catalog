using FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.Domain.Gateways;
using FC.CodeFlix.Catalog.Tests.Shared;

namespace FC.CodeFlix.Catalog.Integration.Tests.Fakes;

public class AdminCatalogFake : IAdminCatalogGateway
{
    private readonly GenreDataGenerator _genreDataGenerator = new();
    private readonly VideoDataGenerator _videoDataGenerator = new();

    public Task<Domain.Entity.Genre> GetGenreAsync(Guid id, CancellationToken cancellationToken)
        => Task.FromResult(_genreDataGenerator.GetValidGenre(id));

    public Task<Video> GetVideoAsync(Guid id, CancellationToken cancellationToken)
    => Task.FromResult(_videoDataGenerator.GetValidVideo(id));
}