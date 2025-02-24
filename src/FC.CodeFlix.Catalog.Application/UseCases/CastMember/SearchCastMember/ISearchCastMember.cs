using FC.CodeFlix.Catalog.Application.Common;
using FC.CodeFlix.Catalog.Application.UseCases.CastMember.Common;
using MediatR;

namespace FC.CodeFlix.Catalog.Application.UseCases.CastMember.SearchCastMember;

public interface ISearchCastMember : IRequestHandler<SearchCastMemberInput, SearchListOutput<CastMemberModelOutput>>
{
}
