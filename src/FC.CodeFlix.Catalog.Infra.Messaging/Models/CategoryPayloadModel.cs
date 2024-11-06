using FC.CodeFlix.Catalog.Application.UseCases.Category.DeleteCategory;
using FC.CodeFlix.Catalog.Application.UseCases.Category.SaveCategory;

namespace FC.CodeFlix.Catalog.Infra.Messaging.Models;

public class CategoryPayloadModel
{
    public Guid Id { get;  set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Name { get;  set; }

    public string Description { get;  set; } = null!;

    public SaveCategoryInput ToSaveCategoryInput()
        => new(Id,Name,Description,CreatedAt,IsActive);

    public DeleteCategoryInput ToDeleteCategoryInput()
        => new (Id);
}
