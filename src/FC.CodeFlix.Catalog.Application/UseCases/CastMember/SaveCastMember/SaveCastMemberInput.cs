using FC.CodeFlix.Catalog.Application.UseCases.CastMember.Common;
using FC.CodeFlix.Catalog.Domain.Enums;
using MediatR;

namespace FC.CodeFlix.Catalog.Application.UseCases.CastMember.SaveCastMember;

public class SaveCastMemberInput:IRequest<CastMemberModelOutput>
{
    public SaveCastMemberInput(Guid id, string name, CastMemberType type, DateTime createdAt)
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


    public Domain.Entity.CastMember ToCastMember()
        => new (Id, Name, Type, CreatedAt);
}