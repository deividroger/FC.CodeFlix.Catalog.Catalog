using FC.CodeFlix.Catalog.Application.Common;
using FC.CodeFlix.Catalog.Application.UseCases.Video.Common;
using FC.CodeFlix.Catalog.Domain.Repositories;

namespace FC.CodeFlix.Catalog.Application.UseCases.Video.SearchVideo;

public class SearchVideo : ISearchVideo
{
    private readonly IVideoRepository _repository;

    public SearchVideo(IVideoRepository repository)
    {
        _repository = repository;
    }

    public async Task<SearchListOutput<VideoModelOutput>> Handle(SearchVideoInput request, CancellationToken cancellationToken)
    {
        var searchInput = request.ToSeachInput();
        var videos = await _repository.SearchAsync(searchInput, cancellationToken);

        return new SearchListOutput<VideoModelOutput>(videos.CurrentPage, videos.PerPage, videos.Total, videos.Items.Select(VideoModelOutput.FromVideo).ToList());
    }
}
