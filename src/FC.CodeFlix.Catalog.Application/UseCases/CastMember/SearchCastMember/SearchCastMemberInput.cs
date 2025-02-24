using FC.CodeFlix.Catalog.Application.Common;
using FC.CodeFlix.Catalog.Application.UseCases.CastMember.Common;
using FC.CodeFlix.Catalog.Domain.Repositories.DTOs;
using MediatR;

namespace FC.CodeFlix.Catalog.Application.UseCases.CastMember.SearchCastMember;

public class SearchCastMemberInput : SearchListInput, IRequest<SearchListOutput<CastMemberModelOutput>>
{
    public SearchCastMemberInput(int page = 1,
                              int perPage = 20,
                              string search = "",
                              string orderBy = "",
                              SearchOrder order = SearchOrder.ASC)
        : base(page, perPage, search, orderBy, order)
    {
    }
}

