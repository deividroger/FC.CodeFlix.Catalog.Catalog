using FC.CodeFlix.Catalog.Domain.Exceptions;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.CodeFlix.Catalog.Integration.Tests.Category.SaveCategory;

[Collection(nameof(SaveCategoryTestFixture))]
public class SaveCategoryTest: IDisposable
{
    private readonly SaveCategoryTestFixture _fixture;

    public SaveCategoryTest(SaveCategoryTestFixture fixture )
    {
        _fixture = fixture;
    }

    [Fact(DisplayName =nameof(SaveCategory_When_InputIsValid_Persists_Category))]
    [Trait("Integration","[UseCase] SaveCategory")]
    public async Task SaveCategory_When_InputIsValid_Persists_Category()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = serviceProvider.GetRequiredService<IElasticClient>();

        var input = _fixture.GetValidInput();

        var output = await mediator.Send(input);

        var persisted = await elasticClient.GetAsync<CategoryModel>(input.Id);

        persisted.Found.Should().BeTrue();

        var document = persisted.Source;
        document.Should().NotBeNull();

        document.Id.Should().Be(input.Id);
        document.Name.Should().Be(input.Name);
        document.Description.Should().Be(input.Description);
        document.IsActive.Should().Be(input.IsActive);
        document.CreatedAt.Should().Be(input.CreatedAt);


        output.Id.Should().Be(input.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().Be(input.CreatedAt);


    }

    [Fact(DisplayName = nameof(SaveCategory_When_InputIsInValid_ThrowsException))]
    [Trait("Integration", "[UseCase] SaveCategory")]
    public async Task SaveCategory_When_InputIsInValid_ThrowsException()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = serviceProvider.GetRequiredService<IElasticClient>();

        var expectedMessage = "Name should not be empty or null";

        var input = _fixture.GetInValidInput();

        var action = async() => await mediator.Send(input);

        await action.Should().ThrowAsync<EntityValidationException>()
            .WithMessage(expectedMessage);

        var persisted = await elasticClient.GetAsync<CategoryModel>(input.Id);

        persisted.Found.Should().BeFalse();

      
    }

    public void Dispose() => _fixture.DeleteAll();
}
