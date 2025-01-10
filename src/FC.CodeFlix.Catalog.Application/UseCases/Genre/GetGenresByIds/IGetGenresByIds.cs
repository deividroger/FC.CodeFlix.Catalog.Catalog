using FC.CodeFlix.Catalog.Application.UseCases.Genre.Common;
using MediatR;

namespace FC.CodeFlix.Catalog.Application.UseCases.Genre.GetGenresByIds;

public  interface IGetGenresByIds: IRequestHandler<GetGenresByIdsInput,IEnumerable<GenreModelOutput>>
{
}
