using FC.CodeFlix.Catalog.Infra.ES.Models;
using FC.CodeFlix.Catalog.Integration.Tests.Common;
using FC.CodeFlix.Catalog.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;
namespace FC.CodeFlix.Catalog.Integration.Tests.Category.Common;

public class CategoryTestFixure : BaseFixture, IDisposable
{
    public CategoryDataGenerator DataGenerator { get; }
    
    public IElasticClient ElasticClient { get; }

    public CategoryTestFixure() : base()
    {
        DataGenerator = new CategoryDataGenerator();
        ElasticClient = ServiceProvider.GetRequiredService<IElasticClient>();

        ElasticSearchOperations.CreateCategoryIndexAsync(ElasticClient).GetAwaiter().GetResult();
    }
    
    public DomainEntity.Category GetValidCategory()
        => DataGenerator.GetValidCategory();

    public IList<CategoryModel> GetCategoryModelList(int count = 10)
        => DataGenerator.GetCategoryModelList(count);

    public void DeleteAll()
        => ElasticSearchOperations.DeleteCategoryDocuments(ElasticClient);

    public void Dispose()
        => ElasticSearchOperations.DeleteCategoryIndex(ElasticClient);
}

[CollectionDefinition(nameof(CategoryTestFixure))]
public class CategoryTestFixtureCollection : ICollectionFixture<CategoryTestFixure>
{ }
