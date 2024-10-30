
using FC.CodeFlix.Catalog.Domain.Validation;

namespace FC.CodeFlix.Catalog.Domain.Entity;

public class Category 
{
    public Category(Guid id,
                    string? name, 
                    string? description,
                    DateTime createdAt,
                    bool isActive = true) : base()
    {

        Id = id;    
        Name = name!;
        Description = description!;
        IsActive = isActive;
        CreatedAt = createdAt;

        Validate();
    }

    public Guid Id { get; private set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Name { get; private set; }

    public string Description { get; private set; }

    private void Validate()
    {
        DomainValidation.NotNullOrEmpty(Id, nameof(Id));
        DomainValidation.NotNullOrEmpty(Name, nameof(Name));
        DomainValidation.NotNull(Description, nameof(Description));
    }
}