using FC.CodeFlix.Catalog.Domain.Entity;

namespace FC.CodeFlix.Catalog.Infra.ES.Models;

public class CategoryModel
{
    public Guid Id { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public static CategoryModel FromEntity(Category category)
        => new()
        {
            Id = category.Id,
            IsActive = category.IsActive,
            CreatedAt = category.CreatedAt,
            Name = category.Name,
            Description = category.Description,
        };

    public Category ToEntity()
        => new(Id, Name, Description, CreatedAt, IsActive);
}
