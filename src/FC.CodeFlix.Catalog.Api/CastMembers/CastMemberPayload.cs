using FC.CodeFlix.Catalog.Application.UseCases.CastMember.Common;
using FC.CodeFlix.Catalog.Domain.Enums;

namespace FC.CodeFlix.Catalog.Api.CastMembers;

public class CastMemberPayload
{
    public Guid Id { get; private set; }
    public string Name { get; set; } = null!;

    public CastMemberType Type { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public static CastMemberPayload FromCategoryModelOutput(CastMemberModelOutput castMember)

        => new()
        {
            Id = castMember.Id,
            Name = castMember.Name,
            Type = castMember.Type,
            CreatedAt = castMember.CreatedAt
        };
}
