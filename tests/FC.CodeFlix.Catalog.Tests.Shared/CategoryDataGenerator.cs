﻿using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;

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

    public DomainEntity.Category GetValidCategory()
        => new(Guid.NewGuid(), GetValidCategoryName(), GetValidCategoryDescription(), DateTime.Now, GetRandomBoolean());
}