using FC.CodeFlix.Catalog.Domain.Repositories.DTOs;
using FC.CodeFlix.Catalog.E2ETests.Graphql.Categories.Common;
using FC.CodeFlix.Catalog.Infra.ES.Models;

namespace FC.CodeFlix.Catalog.E2ETests.Graphql.Categories.SearchCategory;

public class SearchCategoryTestFixture: CategoryTestFixture
{
    public IList<CategoryModel> GetCategoryModelList(List<string> categoriesName)
    => DataGenerator.GetCategoryModelList(categoriesName);

    public IList<CategoryModel> CloneCategoriesListOrdered(
        IList<CategoryModel> categoriesList,
        string orderBy,
        SearchOrder direction)
    => DataGenerator.CloneCategoriesListOrdered(categoriesList, orderBy, direction);
}

[CollectionDefinition(nameof(SearchCategoryTestFixture))]
public class SearchCategoryTestFixtureCollection : ICollectionFixture<SearchCategoryTestFixture>
{ }
