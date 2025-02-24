using FC.CodeFlix.Catalog.Application.UseCases.CastMember.SearchCastMember;
using FC.CodeFlix.Catalog.Domain.Repositories.DTOs;
using FC.CodeFlix.Catalog.UnitTests.Application.UseCases.CastMember.Common;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;

namespace FC.CodeFlix.Catalog.UnitTests.Application.UseCases.CastMember.SearchCastMember;

public class SearchCastMemberTestFixture: CastMemberUseCaseTestFixture
{
    public SearchCastMemberInput GetSearchInput()
    {
        var random = new Random();

        return new SearchCastMemberInput(page: random.Next(1, 10)
                                  , perPage: random.Next(10, 20)
                                  , search: DataGenerator.Faker.Commerce.ProductName()
                                  , orderBy: DataGenerator.Faker.Commerce.ProductName()
                                  , order: random.Next(0, 2) == 0 ? SearchOrder.ASC : SearchOrder.DESC);


    }

    public List<DomainEntity.CastMember> GetCastMemberList(int length = 10)
        => Enumerable.Range(0, length)
                     .Select(_ => GetValidCastMember())
                    .ToList();
}


[CollectionDefinition(nameof(SearchCastMemberTestFixture))]
public class SearchCategoryTestFixtureCollection : ICollectionFixture<SearchCastMemberTestFixture> { }

