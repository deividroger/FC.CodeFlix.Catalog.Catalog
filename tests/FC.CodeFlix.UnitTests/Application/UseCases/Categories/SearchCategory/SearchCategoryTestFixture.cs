﻿using FC.CodeFlix.Catalog.Application.UseCases.Category.SearchCategory;
using FC.CodeFlix.Catalog.Domain.Repositories.DTOs;
using FC.CodeFlix.Catalog.UnitTests.Application.UseCases.Categories.Common;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;

namespace FC.CodeFlix.Catalog.UnitTests.Application.UseCases.Categories.SearchCategory;

public class SearchCategoryTestFixture
    : CategoryUseCaseFixture
{
    public SearchCategoryInput GetSearchInput()
    {
        var random = new Random();

        return new SearchCategoryInput(page: random.Next(1, 10)
                                  , perPage: random.Next(10, 20)
                                  , search: DataGenerator.Faker.Commerce.ProductName()
                                  , orderBy: DataGenerator.Faker.Commerce.ProductName()
                                  , order: random.Next(0, 2) == 0 ? SearchOrder.ASC : SearchOrder.DESC);


    }


    public List<DomainEntity.Category> GetCategoryList(int length = 10)
        => Enumerable.Range(0, length)
                     .Select(_ => GetValidCategory())
                    .ToList();
}

[CollectionDefinition(nameof(SearchCategoryTestFixture))]
public class SearchCategoryTestFixtureCollection : ICollectionFixture<SearchCategoryTestFixture> { }