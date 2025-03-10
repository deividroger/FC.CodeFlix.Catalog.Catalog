using FC.Codeflix.Catalog.E2ETests;
using FC.CodeFlix.Catalog.E2ETests.Base.Fixture;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using RepositoriesDTOs = FC.CodeFlix.Catalog.Domain.Repositories.DTOs;

namespace FC.CodeFlix.Catalog.E2ETests.Graphql.CastMembers.SearchCastMember;

public class SearchCastMemberTestFixture: CastMemberTestFixtureBase, IDisposable
{
    public CatalogClient GraphQLClient { get; }

    public SearchCastMemberTestFixture() : base()
        => GraphQLClient = WebAppFactory.GraphqlClient;

    public IList<CastMemberModel> GetCastMemberModelList(List<string> castMemberNames)
  => DataGenerator.GetCastMemberModelList(castMemberNames);

    public IList<CastMemberModel> CloneCastMemberListOrdered(
        IList<CastMemberModel> castMemberList,
        string orderBy,
        RepositoriesDTOs.SearchOrder direction)
    => DataGenerator.CloneCastMemberListOrdered(castMemberList, orderBy, direction);

}

[CollectionDefinition(nameof(SearchCastMemberTestFixture))]
public class SearchCastMemberTestFixtureCollection : ICollectionFixture<SearchCastMemberTestFixture>
{ }

