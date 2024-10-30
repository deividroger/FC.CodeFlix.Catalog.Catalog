using FC.CodeFlix.Catalog.Application.UseCases.Category.SearchCategory;
using FC.CodeFlix.Catalog.Domain.Repositories.DTOs;
using MediatR;

namespace FC.CodeFlix.Catalog.Api.Categories;

[ExtendObjectType(OperationTypeNames.Query)]
public class CategoryQueries
{
    public async Task<SearchCategoryPayload> GetCategoriesAsync(
        [Service] IMediator mediator,
        int page = 1, 
        int perPage = 10,
        string search = "",
        string sort = "",
        SearchOrder direction = SearchOrder.ASC,
        CancellationToken cancellationToken = default)
    {
        var input = new SearchCategoryInput(page, perPage, search, sort, direction);

        var output = await mediator.Send(input, cancellationToken);

        return SearchCategoryPayload.FromSearchListOutput(output);
    }
}
