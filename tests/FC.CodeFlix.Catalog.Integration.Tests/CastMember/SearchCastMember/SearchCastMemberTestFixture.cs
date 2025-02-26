using FC.CodeFlix.Catalog.Domain.Repositories.DTOs;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using FC.CodeFlix.Catalog.Integration.Tests.CastMember.Common;

namespace FC.CodeFlix.Catalog.Integration.Tests.CastMember.SearchCastMember;

public class SearchCastMemberTestFixture : CastMemberTestFixture
{
    public IList<CastMemberModel> GetCategoryModelList(List<string> castMembersName)
        => DataGenerator.GetCastMemberModelList(castMembersName);

    public IList<CastMemberModel> CloneCastMemberListOrdered(
        IList<CastMemberModel> castMemberList,
        string orderBy,
        SearchOrder direction)
        => DataGenerator.CloneCastMemberListOrdered(castMemberList, orderBy, direction);
}

[CollectionDefinition(nameof(SearchCastMemberTestFixture))]
public class SearchCastMemberTestFixtureCollection : ICollectionFixture<SearchCastMemberTestFixture> { }
