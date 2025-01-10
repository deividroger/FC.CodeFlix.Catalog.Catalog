using FC.CodeFlix.Catalog.Application.UseCases.Genre.Common;
using FC.CodeFlix.Catalog.Domain.Repositories;

namespace FC.CodeFlix.Catalog.Application.UseCases.Genre.GetGenresByIds;

public class GetGenresByIds : IGetGenresByIds
{
    private readonly IGenreRepository _genreRepository;

    public GetGenresByIds(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<IEnumerable<GenreModelOutput>> Handle(GetGenresByIdsInput request, CancellationToken cancellationToken)
    {
        var genres = await _genreRepository.GetGenresByIdsAsync(request.Ids, cancellationToken);

        return genres.Select(GenreModelOutput.FromGenre).ToList();
    }
}