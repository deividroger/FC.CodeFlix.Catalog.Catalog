using FC.CodeFlix.Catalog.Application.Common;
using FC.CodeFlix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace FC.CodeFlix.Catalog.Application.UseCases.Category.SearchCategory;

internal interface ISearchCategory: IRequestHandler<SearchCategoryInput,SearchListOutput<CategoryModelOutput>>
{

}
