using FC.CodeFlix.Catalog.Api.Common;
using FC.CodeFlix.Catalog.Application.Common;
using FC.CodeFlix.Catalog.Application.UseCases.Video.Common;

namespace FC.CodeFlix.Catalog.Api.Videos;

public class SearchVideoPayload : SearchPayload<VideoPayload>
{
    public static SearchVideoPayload FromSearchListOutput(
        SearchListOutput<VideoModelOutput> output)
        => new()
        {
            CurrentPage = output.CurrentPage,
            PerPage = output.PerPage,
            Total = output.Total,
            Items = output.Items.Select(VideoPayload.FromVideoModelOutput).ToList()
        };
}
