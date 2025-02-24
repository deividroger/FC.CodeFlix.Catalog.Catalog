using FC.CodeFlix.Catalog.Application.Common;
using FC.CodeFlix.Catalog.Application.UseCases.CastMember.Common;
using FC.CodeFlix.Catalog.Domain.Repositories;

namespace FC.CodeFlix.Catalog.Application.UseCases.CastMember.SearchCastMember;

public class SearchCastMember : ISearchCastMember
{
    private readonly ICastMemberRepository _repository;

    public SearchCastMember(ICastMemberRepository repository) 
        => _repository = repository;

    public async Task<SearchListOutput<CastMemberModelOutput>> Handle(SearchCastMemberInput request, CancellationToken cancellationToken)
    {
        var searchInput = request.ToSeachInput();

        var castMember = await _repository.SearchAsync(searchInput, cancellationToken);

        return new SearchListOutput<CastMemberModelOutput>(
                castMember.CurrentPage,
                castMember.PerPage,
                castMember.Total,
                castMember.Items.Select(CastMemberModelOutput.FromCastMember).ToList()
            );
    }
}
