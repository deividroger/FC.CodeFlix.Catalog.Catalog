using FC.CodeFlix.Catalog.Application.Common;
using FC.CodeFlix.Catalog.Application.UseCases.Genre.Common;
using FC.CodeFlix.Catalog.Domain.Repositories.DTOs;
using MediatR;

namespace FC.CodeFlix.Catalog.Application.UseCases.Genre.SearchGenre;

public class SearchGenreInput : SearchListInput, IRequest<SearchListOutput<GenreModelOutput>>
{
    public SearchGenreInput(int page = 1, int perPage = 20, string search = "", string orderBy = "", SearchOrder order = SearchOrder.ASC) 
        : base(page, perPage, search, orderBy, order)
    {
    }
}
