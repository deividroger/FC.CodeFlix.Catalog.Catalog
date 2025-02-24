using FC.CodeFlix.Catalog.Domain.Repositories;

namespace FC.CodeFlix.Catalog.Application.UseCases.CastMember.DeleteCastMember;

public class DeleteCastMember : IDeleteCastMember
{
    private readonly ICastMemberRepository _repository;

    public DeleteCastMember(ICastMemberRepository repository) 
        => _repository = repository;

    public async Task Handle(DeleteCastMemberInput request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id, cancellationToken); 
    }
}
