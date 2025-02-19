using FC.CodeFlix.Catalog.Application.UseCases.Genre.Common;
using FC.CodeFlix.Catalog.Domain.Gateways;
using FC.CodeFlix.Catalog.Domain.Repositories;

namespace FC.CodeFlix.Catalog.Application.UseCases.Genre.SaveGenre;

public class SaveGenre : ISaveGenre
{
    private readonly IGenreRepository _repository;
    private readonly IAdminCatalogGateway _gateway;

    public SaveGenre(IGenreRepository repository, IAdminCatalogGateway gateway)
    {
        _repository = repository;
        _gateway = gateway;
    }

    public async Task<GenreModelOutput> Handle(SaveGenreInput request, CancellationToken cancellationToken)
    {
        var genre = await _gateway.GetGenreAsync(request.Id,cancellationToken);

        await _repository.SaveAsync(genre, cancellationToken);

        return GenreModelOutput.FromGenre(genre);
    }
}
