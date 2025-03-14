using FC.CodeFlix.Catalog.Application.UseCases.Video.Common;
using MediatR;

namespace FC.CodeFlix.Catalog.Application.UseCases.Video.SaveVideo;

public class SaveVideoInput: IRequest<VideoModelOutput>
{
    public SaveVideoInput(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private set; }
}
