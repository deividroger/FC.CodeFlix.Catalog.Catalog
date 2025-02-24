using FC.CodeFlix.Catalog.Domain.Repositories;
using FC.CodeFlix.Catalog.Tests.Shared;
using NSubstitute;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;

namespace FC.CodeFlix.Catalog.UnitTests.Application.UseCases.CastMember.Common;

public class CastMemberUseCaseTestFixture
{
    public CastMemberDataGenerator DataGenerator { get; }

    public CastMemberUseCaseTestFixture()
        => DataGenerator = new CastMemberDataGenerator();

    public ICastMemberRepository GetMockRepository()
        => Substitute.For<ICastMemberRepository>();

    public DomainEntity.CastMember GetValidCastMember()
        => DataGenerator.GetValidCastMember();
}

[CollectionDefinition(nameof(CastMemberUseCaseTestFixture))]
public class CastMemberUseCaseFixtureCollection : ICollectionFixture<CastMemberUseCaseTestFixture> { }