using FC.CodeFlix.Catalog.Application.UseCases.Video.SearchVideo;
using FC.CodeFlix.Catalog.Domain.Repositories.DTOs;
using MediatR;

namespace FC.CodeFlix.Catalog.Api.Videos;

[ExtendObjectType(OperationTypeNames.Query)]
public class VideoQueries
{
    public async Task<SearchVideoPayload> GetVideosAsync(
        [Service] IMediator mediator,
        int page = 1,
        int perPage = 10,
        string search = "",
        string sort = "",
        SearchOrder direction = SearchOrder.ASC,
        CancellationToken cancellationToken = default)
    {
        var input = new SearchVideoInput(page, perPage, search, sort, direction);
        var output = await mediator.Send(input, cancellationToken);
        
        return SearchVideoPayload.FromSearchListOutput(output);
    }
}