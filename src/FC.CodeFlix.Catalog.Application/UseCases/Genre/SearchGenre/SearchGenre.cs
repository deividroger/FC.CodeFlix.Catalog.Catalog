using FC.CodeFlix.Catalog.Application.Common;
using FC.CodeFlix.Catalog.Application.UseCases.Genre.Common;
using FC.CodeFlix.Catalog.Domain.Repositories;

namespace FC.CodeFlix.Catalog.Application.UseCases.Genre.SearchGenre;

public class SearchGenre : ISearchGenre
{
    private readonly IGenreRepository _repository;

    public SearchGenre(IGenreRepository repository)
    {
        _repository = repository;
    }

    public async Task<SearchListOutput<GenreModelOutput>> Handle(SearchGenreInput request, CancellationToken cancellationToken)
    {
        var input = request.ToSeachInput();

        var genres = await _repository.SearchAsync(input, cancellationToken);

        return new SearchListOutput<GenreModelOutput>(
            genres.CurrentPage, 
            genres.PerPage, 
            genres.Total, 
            genres.Items.Select(GenreModelOutput.FromGenre).ToList());

    }
}
