using FC.CodeFlix.Catalog.UnitTests.Application.UseCases.Video.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.Domain.Repositories.DTOs;
using FC.CodeFlix.Catalog.Application.UseCases.Video.SearchVideo;


namespace FC.CodeFlix.Catalog.UnitTests.Application.UseCases.Video.SearchVideo;

public class SearchVideoUseCaseTestFixture : VideoUseCaseTestFixture
{
    public SearchVideoInput GetSearchInput()
    {
        var random = new Random();
        return new SearchVideoInput(
            page: random.Next(1, 10),
            perPage: random.Next(10, 20),
            search: DataGenerator.Faker.Commerce.ProductName(),
            orderBy: DataGenerator.Faker.Commerce.ProductName(),
            order: random.Next(0, 2) == 0
                ? SearchOrder.ASC
                : SearchOrder.DESC);
    }
}

[CollectionDefinition(nameof(SearchVideoUseCaseTestFixture))]
public class SearchVideoTestFixtureCollection
    : ICollectionFixture<SearchVideoUseCaseTestFixture>
{
}