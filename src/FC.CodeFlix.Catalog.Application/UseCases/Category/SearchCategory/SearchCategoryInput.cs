using FC.CodeFlix.Catalog.Application.Common;
using FC.CodeFlix.Catalog.Application.UseCases.Category.Common;
using FC.CodeFlix.Catalog.Domain.Repositories.DTOs;
using MediatR;

namespace FC.CodeFlix.Catalog.Application.UseCases.Category.SearchCategory;

public class SearchCategoryInput : SearchListInput, IRequest<SearchListOutput<CategoryModelOutput>>
{
    public SearchCategoryInput(int page = 1, 
                              int perPage = 20, 
                              string search = "", 
                              string orderBy = "", 
                              SearchOrder order = SearchOrder.ASC) 
        : base(page, perPage, search, orderBy, order)
    {
    }
}
