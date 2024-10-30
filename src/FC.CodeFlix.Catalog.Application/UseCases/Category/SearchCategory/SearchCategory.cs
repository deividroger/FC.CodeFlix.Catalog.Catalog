using FC.CodeFlix.Catalog.Application.Common;
using FC.CodeFlix.Catalog.Application.UseCases.Category.Common;
using FC.CodeFlix.Catalog.Domain.Repositories;

namespace FC.CodeFlix.Catalog.Application.UseCases.Category.SearchCategory;

public class SearchCategory : ISearchCategory
{
    private readonly ICategoryRepository _repository;

    public SearchCategory(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<SearchListOutput<CategoryModelOutput>> Handle(SearchCategoryInput request, CancellationToken cancellationToken)
    {
        var searchInput = request.ToSeachInput();

      var categories =  await   _repository.SearchAsync(searchInput, cancellationToken);

        return new SearchListOutput<CategoryModelOutput>(
                categories.CurrentPage,
                categories.PerPage,
                categories.Total,
                categories.Items.Select(CategoryModelOutput.FromCategory).ToList()
            );
    }
}
