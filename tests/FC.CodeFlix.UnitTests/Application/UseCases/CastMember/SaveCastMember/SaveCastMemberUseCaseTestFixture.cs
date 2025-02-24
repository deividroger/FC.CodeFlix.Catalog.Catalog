using FC.CodeFlix.Catalog.Application.UseCases.CastMember.SaveCastMember;
using FC.CodeFlix.Catalog.UnitTests.Application.UseCases.CastMember.Common;

namespace FC.CodeFlix.Catalog.UnitTests.Application.UseCases.CastMember.SaveCastMember;

public class SaveCastMemberUseCaseTestFixture: CastMemberUseCaseTestFixture
{
    public SaveCastMemberInput GetValidInput()
    {
        var castMember = DataGenerator.GetValidCastMember();
        return new SaveCastMemberInput(castMember.Id,castMember.Name,castMember.Type,castMember.CreatedAt);
    }

    public SaveCastMemberInput GetInValidInput()
    {
        var castMember = DataGenerator.GetValidCastMember();
        return new SaveCastMemberInput(castMember.Id, string.Empty, castMember.Type, castMember.CreatedAt);
    }
}

[CollectionDefinition(nameof(SaveCastMemberUseCaseTestFixture))]
public class SaveCastMemberUseCaseTestFixtureCollection : ICollectionFixture<SaveCastMemberUseCaseTestFixture> { }