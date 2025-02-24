using FC.CodeFlix.Catalog.Application.UseCases.CastMember.Common;
using FC.CodeFlix.Catalog.Domain.Repositories;

namespace FC.CodeFlix.Catalog.Application.UseCases.CastMember.SaveCastMember;

public class SaveCastMember : ISaveCastMember
{
    private readonly ICastMemberRepository _repository;

    public SaveCastMember(ICastMemberRepository repository) => 
        _repository = repository;

    public async Task<CastMemberModelOutput> Handle(SaveCastMemberInput request, CancellationToken cancellationToken)
    {
        var castMember = request.ToCastMember();

        await _repository.SaveAsync(castMember, cancellationToken);

        return CastMemberModelOutput.FromCastMember(castMember);
    }
}
