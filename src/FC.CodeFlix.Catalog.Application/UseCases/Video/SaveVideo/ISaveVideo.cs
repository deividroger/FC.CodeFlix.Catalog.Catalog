using FC.CodeFlix.Catalog.Application.UseCases.Video.Common;
using MediatR;

namespace FC.CodeFlix.Catalog.Application.UseCases.Video.SaveVideo;

public interface ISaveVideo: IRequestHandler<SaveVideoInput, VideoModelOutput>
{
}
