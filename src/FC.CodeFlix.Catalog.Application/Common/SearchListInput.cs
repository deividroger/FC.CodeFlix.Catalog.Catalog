using FC.CodeFlix.Catalog.Domain.Repositories.DTOs;

namespace FC.CodeFlix.Catalog.Application.Common;

public class SearchListInput
{
    public SearchListInput(int page, int perPage, string search, string orderBy, SearchOrder order)
    {
        Page = page;
        PerPage = perPage;
        Search = search;
        OrderBy = orderBy;
        Order = order;
    }

    public int Page { get; set; }

    public int PerPage { get; set; }

    public string Search { get; set; }

    public string OrderBy { get; set; }

    public SearchOrder Order { get; set; }

    public SearchInput ToSeachInput()
        => new SearchInput(Page, PerPage, Search,OrderBy,Order);
        
}
