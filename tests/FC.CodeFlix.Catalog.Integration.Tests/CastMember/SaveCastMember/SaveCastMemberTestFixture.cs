using FC.CodeFlix.Catalog.Application.UseCases.CastMember.SaveCastMember;
using FC.CodeFlix.Catalog.Integration.Tests.CastMember.Common;

namespace FC.CodeFlix.Catalog.Integration.Tests.CastMember.SaveCastMember;

public class SaveCastMemberTestFixture: CastMemberTestFixture
{

    public SaveCastMemberTestFixture() : base()
    {

    }

    public SaveCastMemberInput GetValidInput()
        => new SaveCastMemberInput(
            Guid.NewGuid(),
            DataGenerator.GetValidName(),
            DataGenerator.GetRandomCastMemberType(),
            DateTime.Now);

    public SaveCastMemberInput GetInValidInput()
        => new SaveCastMemberInput(
            Guid.NewGuid(),
            null!,
            DataGenerator.GetRandomCastMemberType(),
            DateTime.Now);
}

[CollectionDefinition(nameof(SaveCastMemberTestFixture))]
public class SaveCastMemberTestFixtureCollection : ICollectionFixture<SaveCastMemberTestFixture> { }