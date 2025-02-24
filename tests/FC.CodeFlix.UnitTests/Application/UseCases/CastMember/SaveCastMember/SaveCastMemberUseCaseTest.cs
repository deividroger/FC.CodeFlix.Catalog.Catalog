using FC.CodeFlix.Catalog.Domain.Exceptions;
using FluentAssertions;
using NSubstitute;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;
using UseCase = FC.CodeFlix.Catalog.Application.UseCases.CastMember.SaveCastMember;

namespace FC.CodeFlix.Catalog.UnitTests.Application.UseCases.CastMember.SaveCastMember;

[Collection(nameof(SaveCastMemberUseCaseTestFixture))]
public class SaveCastMemberUseCaseTest
{
    private readonly SaveCastMemberUseCaseTestFixture _fixture;

    public SaveCastMemberUseCaseTest(SaveCastMemberUseCaseTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(SaveValidCastMember))]
    [Trait("Application", "[UseCase] SaveCastMember")]
    public async Task SaveValidCastMember()
    {
        var repository = _fixture.GetMockRepository();

        var useCase = new UseCase.SaveCastMember(repository);

        var input = _fixture.GetValidInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        await repository.Received(1).SaveAsync(
            Arg.Any<DomainEntity.CastMember>(), Arg.Any<CancellationToken>());

        output.Should().NotBeNull();
        output.Id.Should().Be(input.Id);
        output.Name.Should().Be(input.Name);
        output.Type.Should().Be(input.Type);
        output.CreatedAt.Should().Be(input.CreatedAt);
    }

    [Fact(DisplayName = nameof(SaveInValidCastMember))]
    [Trait("Application", "[UseCase] SaveCastMember")]
    public async Task SaveInValidCastMember()
    {
        var repository = _fixture.GetMockRepository();

        var useCase = new UseCase.SaveCastMember(repository);

        var input = _fixture.GetInValidInput();

        var action = async () => await useCase.Handle(input, CancellationToken.None);

        await repository.DidNotReceive().SaveAsync(
             Arg.Any<DomainEntity.CastMember>(), Arg.Any<CancellationToken>());

        await action.Should().ThrowAsync<EntityValidationException>().WithMessage("Name should not be empty or null");
    }
}