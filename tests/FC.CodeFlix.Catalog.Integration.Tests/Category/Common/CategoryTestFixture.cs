using Elasticsearch.Net;
using FC.CodeFlix.Catalog.Infra.ES;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using FC.CodeFlix.Catalog.Integration.Tests.Common;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;
namespace FC.CodeFlix.Catalog.Integration.Tests.Category.Common;

public class CategoryTestFixure : BaseFixture, IDisposable
{
    public CategoryTestFixure() : base()
    {
        CreateCategoryIndexAsync().GetAwaiter().GetResult();

    }
    private  async Task CreateCategoryIndexAsync()
    {
        var esClient = ServiceProvider.GetRequiredService<IElasticClient>();

        var response = await esClient.Indices.CreateAsync(ElasticsearchIndices.Category, c => c
            .Map<CategoryModel>(m => m
                .Properties(ps => ps
                    .Keyword(t => t
                        .Name(category => category.Id)
                    )
                    .Text(t => t
                        .Name(category => category.Name)
                        .Fields(fs => fs
                            .Keyword(k => k
                                .Name(category => category.Name.Suffix("keyword")))
                        )
                    )
                    .Text(t => t
                        .Name(category => category.Description)
                    )
                    .Boolean(b => b
                        .Name(category => category.IsActive)
                    )
                    .Date(d => d
                        .Name(category => category.CreatedAt)
                    )
                )
            )
        );
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

    public IList<CategoryModel> GetCategoryModelList(int count = 10)
        => Enumerable.Range(0, count)
                        .Select(_ =>
                        {
                            Task.Delay(5).GetAwaiter().GetResult();
                            return CategoryModel.FromEntity(GetValidCategory());
                        })
                        .ToList();
    public void DeleteAll()
    {
        var elasticClient = ServiceProvider.GetRequiredService<IElasticClient>();
        elasticClient.DeleteByQuery<CategoryModel>(del => 
            del.Query(
                q => q.QueryString(qs => qs.Query("*"))
            ).Conflicts(Conflicts.Proceed)
        );
    }

    public void Dispose()
    {
        var elasticClient = ServiceProvider.GetRequiredService<IElasticClient>();

        elasticClient.Indices.Delete(ElasticsearchIndices.Category);
    }
}

[CollectionDefinition(nameof(CategoryTestFixure))]
public class CategoryTestFixtureCollection : ICollectionFixture<CategoryTestFixure>
{ }
