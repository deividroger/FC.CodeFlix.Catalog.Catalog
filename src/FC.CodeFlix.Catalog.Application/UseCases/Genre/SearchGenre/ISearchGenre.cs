using FC.CodeFlix.Catalog.Application.Common;
using FC.CodeFlix.Catalog.Application.UseCases.Genre.Common;
using MediatR;

namespace FC.CodeFlix.Catalog.Application.UseCases.Genre.SearchGenre;

public interface ISearchGenre: IRequestHandler<SearchGenreInput, SearchListOutput<GenreModelOutput>>
{

}
