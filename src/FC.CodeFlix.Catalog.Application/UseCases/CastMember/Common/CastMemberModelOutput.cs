using FC.CodeFlix.Catalog.Domain.Enums;

namespace FC.CodeFlix.Catalog.Application.UseCases.CastMember.Common;

public class CastMemberModelOutput
{
    public CastMemberModelOutput(Guid id, string name, CastMemberType type, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Type = type;
        CreatedAt = createdAt;
    }

    public Guid Id { get; set; }

    public string Name { get; set; }

    public CastMemberType Type { get; set; }

    public DateTime CreatedAt { get; set; }

    public static CastMemberModelOutput FromCastMember(Domain.Entity.CastMember castMember)
        => new(castMember.Id, castMember.Name, castMember.Type, castMember.CreatedAt);
}
