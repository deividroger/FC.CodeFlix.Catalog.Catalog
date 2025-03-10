using FC.CodeFlix.Catalog.Api.Common;
using FC.CodeFlix.Catalog.Application.Common;
using FC.CodeFlix.Catalog.Application.UseCases.Category.Common;

namespace FC.CodeFlix.Catalog.Api.Categories;

public class SearchCategoryPayload : SearchPayload<CategoryPayload>
{
    public static SearchCategoryPayload FromSearchListOutput(SearchListOutput<CategoryModelOutput> output)
    {
        return new SearchCategoryPayload
        {
            CurrentPage = output.CurrentPage,
            PerPage = output.PerPage,
            Total = output.Total,
            Items = output.Items.Select(x => CategoryPayload.FromCategoryModelOutput(x)).ToList(),
        };
    }
}
