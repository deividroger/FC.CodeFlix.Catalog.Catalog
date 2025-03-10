using FC.CodeFlix.Catalog.Application.UseCases.CastMember.SearchCastMember;
using FC.CodeFlix.Catalog.Domain.Repositories.DTOs;
using MediatR;

namespace FC.CodeFlix.Catalog.Api.CastMembers;

[ExtendObjectType(OperationTypeNames.Query)]
public class CastMemberQueries
{
    public async Task<SearchCastMemberPayload> GetCastMembersAsync(
        [Service] IMediator mediator,
        int page = 1, int
        perPage = 10, string
        search = "",
        string sort = "",
        SearchOrder direction = SearchOrder.ASC,
        CancellationToken cancellation = default)
    {
        var input  = new SearchCastMemberInput(page, perPage, search, sort, direction);

        var output = await mediator.Send(input, cancellation);

        return SearchCastMemberPayload.FromSearchListOutput(output);
    }
}
