using FC.CodeFlix.Catalog.Application.UseCases.Category.DeleteCategory;
using FC.CodeFlix.Catalog.Application.UseCases.Category.SaveCategory;
using HotChocolate.Authorization;
using MediatR;

namespace FC.CodeFlix.Catalog.Api.Categories;

[ExtendObjectType(OperationTypeNames.Mutation)]
[Authorize(Roles = new[] { "catalog_admin" })]
public class CategoriesMutations
{
    public async Task<CategoryPayload> SaveCategoryAsync(SaveCategoryInput input, [Service] IMediator mediator, CancellationToken cancellation)
    {
        var output = await mediator.Send(input, cancellation);

        return CategoryPayload.FromCategoryModelOutput(output);
    }

    public async Task<bool> DeleteCategoryAsync(Guid id, [Service] IMediator mediator, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteCategoryInput(id), cancellationToken);

        return true;
    }
}
