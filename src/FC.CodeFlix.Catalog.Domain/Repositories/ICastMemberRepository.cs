using FC.CodeFlix.Catalog.Domain.Entity;

namespace FC.CodeFlix.Catalog.Domain.Repositories;

public interface ICastMemberRepository : IRepository<CastMember>
{
    Task<IEnumerable<CastMember>> GetCastMembersByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
}
