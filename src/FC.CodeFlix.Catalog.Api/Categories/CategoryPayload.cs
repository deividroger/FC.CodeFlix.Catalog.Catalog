using FC.CodeFlix.Catalog.Application.UseCases.Category.Common;

namespace FC.CodeFlix.Catalog.Api.Categories;

public class CategoryPayload
{
    public Guid Id { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public static CategoryPayload FromCategoryModelOutput(CategoryModelOutput category)
    
        => new()
        {
            Id = 
            category.Id,
            Name = category.Name,
            Description = category.Description, 
            CreatedAt =  category.CreatedAt,
            IsActive =   category.IsActive
        };
    
}
