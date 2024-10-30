using FC.CodeFlix.Catalog.Application.UseCases.Category.Common;
using FC.CodeFlix.Catalog.Domain.Repositories;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;
namespace FC.CodeFlix.Catalog.Application.UseCases.Category.SaveCategory;

public class SaveCategory : ISaveCategory
{
    private readonly ICategoryRepository _repository;

    public SaveCategory(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<CategoryModelOutput> Handle(SaveCategoryInput request, CancellationToken cancellationToken)
    {
        var category = new DomainEntity.Category(request.Id, request.Name, request.Description, request.CreatedAt, request.IsActive);

        await _repository.SaveAsync(category, cancellationToken);

        return CategoryModelOutput.FromCategory(category);
    }
}
