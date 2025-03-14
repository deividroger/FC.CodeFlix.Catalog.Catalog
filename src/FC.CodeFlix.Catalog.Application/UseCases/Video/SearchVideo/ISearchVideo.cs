using FC.CodeFlix.Catalog.Application.Common;
using FC.CodeFlix.Catalog.Application.UseCases.Video.Common;
using MediatR;

namespace FC.CodeFlix.Catalog.Application.UseCases.Video.SearchVideo;

public interface ISearchVideo : IRequestHandler<SearchVideoInput, SearchListOutput<VideoModelOutput>>
{
}
