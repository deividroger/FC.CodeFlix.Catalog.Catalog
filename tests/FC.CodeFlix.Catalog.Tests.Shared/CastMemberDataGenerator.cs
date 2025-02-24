using FC.CodeFlix.Catalog.Domain.Enums;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;
namespace FC.CodeFlix.Catalog.Tests.Shared;

public class CastMemberDataGenerator: DataGeneratorBase
{
    public DomainEntity.CastMember GetValidCastMember()
        => new(
            Guid.NewGuid(),
            GetValidName(),
            GetRandomCastMemberType()
            );

    public string GetValidName()
        => Faker.Name.FullName();

    public CastMemberType GetRandomCastMemberType()
        => (CastMemberType)new Random().Next(1,2);
}
