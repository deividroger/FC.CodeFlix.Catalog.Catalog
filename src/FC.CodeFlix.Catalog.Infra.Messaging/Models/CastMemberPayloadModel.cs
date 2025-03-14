using FC.CodeFlix.Catalog.Application.UseCases.CastMember.DeleteCastMember;
using FC.CodeFlix.Catalog.Application.UseCases.CastMember.SaveCastMember;
using FC.CodeFlix.Catalog.Application.UseCases.Category.DeleteCategory;
using FC.CodeFlix.Catalog.Application.UseCases.Category.SaveCategory;
using FC.CodeFlix.Catalog.Domain.Enums;

namespace FC.CodeFlix.Catalog.Infra.Messaging.Models;

public class CastMemberPayloadModel
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Name { get; set; } = null!;

    public CastMemberType Type { get; set; }

    public SaveCastMemberInput ToSaveCastMemberInput()
        => new(Id, Name, Type, CreatedAt);

    public DeleteCastMemberInput ToDeleteCastMemberInput()
        => new(Id);
}
