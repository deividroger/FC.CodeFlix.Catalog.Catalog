using FC.CodeFlix.Catalog.Application.UseCases.CastMember.DeleteCastMember;
using FC.CodeFlix.Catalog.Domain.Exceptions;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using FC.CodeFlix.Catalog.Integration.Tests.CastMember.Common;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.CodeFlix.Catalog.Integration.Tests.CastMember.DeleteCastMember;

[Collection(nameof(CastMemberTestFixture))]
public class DeleteCastMemberTest : IDisposable
{
    private readonly CastMemberTestFixture _fixture;

    public DeleteCastMemberTest(CastMemberTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(DeleteCastMemberWhenReceivesAndExistingId_DeletesCastMember))]
    [Trait("Integration", "[UseCase] DeleteCastMember")]
    public async Task DeleteCastMemberWhenReceivesAndExistingId_DeletesCastMember()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var elasticClient = _fixture.ElasticClient;

        var castMembersExample = _fixture.GetCastMemberModelList();

        await elasticClient.IndexManyAsync(castMembersExample);

        var input = new DeleteCastMemberInput(castMembersExample[3].Id);

        await mediator.Send(input, CancellationToken.None);

        var deletedCategory = await elasticClient.GetAsync<CastMemberModel>(input.Id);

        deletedCategory.Found.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(DeleteCastMemberWhenReceivesAndNonExistingId_ThrowsException))]
    [Trait("Integration", "[UseCase] DeleteCastMember")]
    public async Task DeleteCastMemberWhenReceivesAndNonExistingId_ThrowsException()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = _fixture.ElasticClient;

        var castMembers = _fixture.GetCastMemberModelList();

        await elasticClient.IndexManyAsync(castMembers);

        var input = new DeleteCastMemberInput(Guid.NewGuid());

        var action = async () => await mediator.Send(input, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>()
                    .WithMessage($"CastMember '{input.Id}' not found.");
    }

    public void Dispose()
        => _fixture.DeleteAll();
}
