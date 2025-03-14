using FC.CodeFlix.Catalog.Application.Common;
using FC.CodeFlix.Catalog.Application.UseCases.Video.Common;
using FC.CodeFlix.Catalog.Domain.Repositories.DTOs;
using MediatR;

namespace FC.CodeFlix.Catalog.Application.UseCases.Video.SearchVideo;

public class SearchVideoInput : SearchListInput, IRequest<SearchListOutput<VideoModelOutput>>
{
    public SearchVideoInput(int page = 1, int perPage = 20, string search = "", string orderBy = "", SearchOrder order = SearchOrder.ASC)
        : base(page, perPage, search, orderBy, order)
    {
    }
}
