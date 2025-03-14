using FC.CodeFlix.Catalog.Application.UseCases.Video.Common;
using FC.CodeFlix.Catalog.Domain.Gateways;
using FC.CodeFlix.Catalog.Domain.Repositories;

namespace FC.CodeFlix.Catalog.Application.UseCases.Video.SaveVideo;

public class SaveVideo : ISaveVideo
{
    private readonly IVideoRepository _videoRepository;
    private readonly IAdminCatalogGateway _gateway;

    public SaveVideo(IVideoRepository videoRepository, IAdminCatalogGateway gateway)
    {
        _videoRepository = videoRepository;
        _gateway = gateway;
    }

    public async Task<VideoModelOutput> Handle(SaveVideoInput request, CancellationToken cancellationToken)
    {
        var video = await _gateway.GetVideoAsync(request.Id, cancellationToken);

        await _videoRepository.SaveAsync(video, cancellationToken);

        return VideoModelOutput.FromVideo(video);
    }
}
