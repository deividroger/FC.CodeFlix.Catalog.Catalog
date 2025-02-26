using FC.CodeFlix.Catalog.Domain.Exceptions;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FC.CodeFlix.Catalog.Integration.Tests.CastMember.SaveCastMember;


[Collection(nameof(SaveCastMemberTestFixture))]
public class SaveCastMemberTest : IDisposable
{
    private readonly SaveCastMemberTestFixture _fixture;

    public SaveCastMemberTest(SaveCastMemberTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(SaveCastMember_When_InputIsValid_Persists_CastMember))]
    [Trait("Integration", "[UseCase] SaveCastMember")]
    public async Task SaveCastMember_When_InputIsValid_Persists_CastMember()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = _fixture.ElasticClient;

        var input = _fixture.GetValidInput();

        var output = await mediator.Send(input);

        var persisted = await elasticClient.GetAsync<CastMemberModel>(input.Id);

        persisted.Found.Should().BeTrue();

        var document = persisted.Source;
        document.Should().NotBeNull();

        document.Id.Should().Be(input.Id);
        document.Name.Should().Be(input.Name);
        document.Type.Should().Be(input.Type);
        document.CreatedAt.Should().Be(input.CreatedAt);


        output.Id.Should().Be(input.Id);
        output.Name.Should().Be(input.Name);
        output.Type.Should().Be(input.Type);
        output.CreatedAt.Should().Be(input.CreatedAt);


    }

    [Fact(DisplayName = nameof(SaveCastMember_When_InputIsInValid_ThrowsException))]
    [Trait("Integration", "[UseCase] SaveCategory")]
    public async Task SaveCastMember_When_InputIsInValid_ThrowsException()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = _fixture.ElasticClient;

        var expectedMessage = "Name should not be empty or null";

        var input = _fixture.GetInValidInput();

        var action = async () => await mediator.Send(input);

        await action.Should().ThrowAsync<EntityValidationException>()
            .WithMessage(expectedMessage);

        var persisted = await elasticClient.GetAsync<CastMemberModel>(input.Id);

        persisted.Found.Should().BeFalse();
    }

    public void Dispose() => _fixture.DeleteAll();
}
