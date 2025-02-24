using FC.CodeFlix.Catalog.Tests.Shared;

namespace FC.CodeFlix.Catalog.UnitTests.Domain.Entity.CastMember;

public class CastMemberTestFixture
{
    private readonly CastMemberDataGenerator _dataGenerator = new();

    public Catalog.Domain.Entity.CastMember GetValidCastMember()
        => _dataGenerator.GetValidCastMember();

}


[CollectionDefinition(nameof(CastMemberTestFixture))]
public class CastMemberTestFixtureCollection: ICollectionFixture<CastMemberTestFixture>
{

}