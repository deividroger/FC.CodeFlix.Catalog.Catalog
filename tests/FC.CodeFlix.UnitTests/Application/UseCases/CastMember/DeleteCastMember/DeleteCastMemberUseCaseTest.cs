﻿using FC.CodeFlix.Catalog.UnitTests.Application.UseCases.CastMember.Common;
using NSubstitute;
using UseCase = FC.CodeFlix.Catalog.Application.UseCases.CastMember.DeleteCastMember;

namespace FC.CodeFlix.Catalog.UnitTests.Application.UseCases.CastMember.DeleteCastMember;

[Collection(nameof(CastMemberUseCaseTestFixture))]
public class DeleteCastMemberUseCaseTest
{
    private readonly CastMemberUseCaseTestFixture _fixture;

    public DeleteCastMemberUseCaseTest(CastMemberUseCaseTestFixture fixture) 
        => _fixture = fixture;

    [Fact(DisplayName = nameof(DeleteCastMember))]
    [Trait("Application", "[UseCase] DeleteCastMember")]
    public async Task DeleteCastMember()
    {
        var repository = _fixture.GetMockRepository();

        var useCase = new UseCase.DeleteCastMember(repository);

        var input = new UseCase.DeleteCastMemberInput(Guid.NewGuid());

        await useCase.Handle(input, CancellationToken.None);

        await repository.Received(1).DeleteAsync(input.Id, Arg.Any<CancellationToken>());
    }
}
