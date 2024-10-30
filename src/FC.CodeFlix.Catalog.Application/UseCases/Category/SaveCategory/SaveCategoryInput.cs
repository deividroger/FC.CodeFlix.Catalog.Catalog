using FC.CodeFlix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace FC.CodeFlix.Catalog.Application.UseCases.Category.SaveCategory;

public class SaveCategoryInput: IRequest<CategoryModelOutput>
{
    public SaveCategoryInput(Guid id,
                   string? name,
                   string? description,
                   DateTime createdAt,
                   bool isActive) 
    {

        Id = id;
        Name = name!;
        Description = description!;
        IsActive = isActive;
        CreatedAt = createdAt;

        
    }

    public Guid Id { get;  set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Name { get;  set; }

    public string Description { get;  set; }

}
