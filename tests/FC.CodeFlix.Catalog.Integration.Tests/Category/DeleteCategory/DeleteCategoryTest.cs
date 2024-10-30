using FC.CodeFlix.Catalog.Application.UseCases.Category.DeleteCategory;
using FC.CodeFlix.Catalog.Domain.Exceptions;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using FC.CodeFlix.Catalog.Integration.Tests.Category.Common;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.CodeFlix.Catalog.Integration.Tests.Category.DeleteCategory;

[Collection(nameof(CategoryTestFixure))]
public class DeleteCategoryTest : IDisposable
{
    private readonly CategoryTestFixure _fixture;

    public DeleteCategoryTest(CategoryTestFixure fixture)
        => _fixture = fixture;

    [Fact(DisplayName =nameof(DeleteCategoryWhenReceivesAndExistingId_DeletesCategory))]
    [Trait("Integration", "[UseCase] DeleteCategory")]
    public async Task  DeleteCategoryWhenReceivesAndExistingId_DeletesCategory()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        
        var elasticClient = _fixture.ElasticClient;

        var categoriesExample = _fixture.GetCategoryModelList();

        await elasticClient.IndexManyAsync(categoriesExample);

        var input = new DeleteCategoryInput(categoriesExample[3].Id);

        await mediator.Send(input, CancellationToken.None);

        var deletedCategory = await elasticClient.GetAsync<CategoryModel>(input.Id);

        deletedCategory.Found.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(DeleteCategoryWhenReceivesAndNonExistingId_ThrowsException))]
    [Trait("Integration", "[UseCase] DeleteCategory")]
    public async Task DeleteCategoryWhenReceivesAndNonExistingId_ThrowsException()
    {
        var serviceProvider = _fixture.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = _fixture.ElasticClient;

        var categoriesExample = _fixture.GetCategoryModelList();

        await elasticClient.IndexManyAsync(categoriesExample);

        var input = new DeleteCategoryInput(Guid.NewGuid());

       var action = async () =>  await mediator.Send(input, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>()
                    .WithMessage($"Category '{input.Id}' not found.");
    }

    public void Dispose()
        => _fixture.DeleteAll();
}
