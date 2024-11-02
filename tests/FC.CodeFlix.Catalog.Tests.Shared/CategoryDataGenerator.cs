using FC.CodeFlix.Catalog.Domain.Repositories.DTOs;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;

namespace FC.CodeFlix.Catalog.Tests.Shared;

public class CategoryDataGenerator: DataGeneratorBase
{
    public CategoryDataGenerator() : base() 
    {
        
    }

    public string GetValidCategoryName()
    {
        var categoryName = "";

        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];

        if (categoryName.Length > 255)
            categoryName = categoryName[..255];

        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();


        if (categoryDescription.Length > 10_000)
            categoryDescription = categoryDescription[..10_000];


        return categoryDescription;
    }

    public IList<CategoryModel> GetCategoryModelList(int count = 10)
        => Enumerable.Range(0, count)
                        .Select(_ =>
                        {
                            Task.Delay(5).GetAwaiter().GetResult();
                            return CategoryModel.FromEntity(GetValidCategory());
                        })
                        .ToList();

    public IList<CategoryModel> GetCategoryModelList(List<string> categoriesName)
    => categoriesName.Select(name =>
    {
        Task.Delay(5).GetAwaiter().GetResult();
        var category = CategoryModel.FromEntity(GetValidCategory());
        category.Name = name;
        return category;
    }).ToList();

    public IList<CategoryModel> CloneCategoriesListOrdered(
        IList<CategoryModel> categoriesList,
        string orderBy,
        SearchOrder direction)
    {
        var listClone = new List<CategoryModel>(categoriesList);
        var orderedEnumerable = (orderBy.ToLower(), direction) switch
        {
            ("name", SearchOrder.ASC) => listClone.OrderBy(x => x.Name)
                .ThenBy(x => x.Id),
            ("name", SearchOrder.DESC) => listClone.OrderByDescending(x => x.Name)
                .ThenByDescending(x => x.Id),
            ("id", SearchOrder.ASC) => listClone.OrderBy(x => x.Id),
            ("id", SearchOrder.DESC) => listClone.OrderByDescending(x => x.Id),
            ("createdat", SearchOrder.ASC) => listClone.OrderBy(x => x.CreatedAt),
            ("createdat", SearchOrder.DESC) => listClone.OrderByDescending(x => x.CreatedAt),
            _ => listClone.OrderBy(x => x.Name).ThenBy(x => x.Id),
        };
        return orderedEnumerable.ToList();
    }

    public DomainEntity.Category GetValidCategory()
        => new(Guid.NewGuid(), GetValidCategoryName(), GetValidCategoryDescription(), DateTime.UtcNow.Date, GetRandomBoolean());
}
