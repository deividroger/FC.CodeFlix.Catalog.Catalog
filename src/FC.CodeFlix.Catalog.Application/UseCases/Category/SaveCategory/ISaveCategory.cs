using FC.CodeFlix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace FC.CodeFlix.Catalog.Application.UseCases.Category.SaveCategory
{
    public interface ISaveCategory: IRequestHandler<SaveCategoryInput,CategoryModelOutput>
    {
    }
}
