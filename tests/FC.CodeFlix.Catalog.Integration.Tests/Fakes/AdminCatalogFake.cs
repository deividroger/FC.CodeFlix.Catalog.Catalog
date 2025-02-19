using FC.CodeFlix.Catalog.Domain.Gateways;
using FC.CodeFlix.Catalog.Tests.Shared;

namespace FC.CodeFlix.Catalog.Integration.Tests.Fakes;

public class AdminCatalogFake : IAdminCatalogGateway
{
    private readonly GenreDataGenerator _dataGenerator = new();

    public Task<Domain.Entity.Genre> GetGenreAsync(Guid id, CancellationToken cancellationToken)
        => Task.FromResult(_dataGenerator.GetValidGenre(id));
}