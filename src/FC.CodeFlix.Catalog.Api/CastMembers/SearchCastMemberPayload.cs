using FC.CodeFlix.Catalog.Api.Common;
using FC.CodeFlix.Catalog.Application.Common;
using FC.CodeFlix.Catalog.Application.UseCases.CastMember.Common;

namespace FC.CodeFlix.Catalog.Api.CastMembers;

public class SearchCastMemberPayload : SearchPayload<CastMemberPayload>
{
    public static SearchCastMemberPayload FromSearchListOutput(SearchListOutput<CastMemberModelOutput> output)
    {
        return new SearchCastMemberPayload
        {
            CurrentPage = output.CurrentPage,
            PerPage = output.PerPage,
            Total = output.Total,
            Items = output.Items.Select(x => CastMemberPayload.FromCategoryModelOutput(x)).ToList(),
        };
    }
}