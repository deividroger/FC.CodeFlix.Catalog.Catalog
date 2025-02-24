using FC.CodeFlix.Catalog.Domain.Enums;
using FC.CodeFlix.Catalog.Domain.Validation;

namespace FC.CodeFlix.Catalog.Domain.Entity;

public class CastMember
{
    public CastMember(Guid id, string name, CastMemberType type, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Type = type;
        CreatedAt = createdAt;

        Validate();
    }

    public CastMember(Guid id, string name, CastMemberType type) :
        this(id, name, type, DateTime.Now)
    {

    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public CastMemberType Type { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private void Validate()
    {
        DomainValidation.NotNullOrEmpty(Id, nameof(Id));
        DomainValidation.NotNullOrEmpty(Name, nameof(Name));
        DomainValidation.IdDefined(Type, nameof(Type));
    }
}
