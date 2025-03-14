using FC.CodeFlix.Catalog.Domain.Entity;

namespace FC.CodeFlix.Catalog.Domain.Gateways;

public interface IAdminCatalogGateway
{
    Task<Genre> GetGenreAsync(Guid id, CancellationToken cancellationToken);

    Task<Video> GetVideoAsync(Guid id, CancellationToken cancellationToken);
}
