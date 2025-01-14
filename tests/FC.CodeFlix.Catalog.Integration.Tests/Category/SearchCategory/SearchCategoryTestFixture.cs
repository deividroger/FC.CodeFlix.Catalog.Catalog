﻿using FC.CodeFlix.Catalog.Domain.Repositories.DTOs;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using FC.CodeFlix.Catalog.Integration.Tests.Category.Common;

namespace FC.CodeFlix.Catalog.Integration.Tests.Category.SearchCategory;

public class SearchCategoryTestFixture: CategoryTestFixure
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
public class SearchCategoryTestFixtureCollection : ICollectionFixture<SearchCategoryTestFixture> { }
