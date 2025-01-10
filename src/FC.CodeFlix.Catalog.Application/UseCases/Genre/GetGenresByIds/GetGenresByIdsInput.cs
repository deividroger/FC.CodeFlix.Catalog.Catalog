using FC.CodeFlix.Catalog.Application.UseCases.Genre.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC.CodeFlix.Catalog.Application.UseCases.Genre.GetGenresByIds;

public class GetGenresByIdsInput: IRequest<IEnumerable<GenreModelOutput>>
{
    public GetGenresByIdsInput(IEnumerable<Guid> ids)
    {
        Ids = ids;
    }

    public IEnumerable<Guid> Ids { get; }
}
