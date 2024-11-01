using FC.CodeFlix.Catalog.E2ETests.Graphql.Categories.Common;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using FluentAssertions;
using Nest;

namespace FC.CodeFlix.Catalog.E2ETests.Graphql.Categories.DeleteCategory;

[Collection(nameof(CategoryTestFixture))]
public class DeleteCategoryTest:IDisposable
{
    private readonly CategoryTestFixture _fixture;

    public DeleteCategoryTest(CategoryTestFixture categoryTestFixture)
        => _fixture = categoryTestFixture;

    [Fact(DisplayName = nameof(DeleteCategoryWhenReceivesAndExistingId_DeletesCategory))]
    [Trait("E2E/GraphQL", "[Category] Save")]
    public async Task DeleteCategoryWhenReceivesAndExistingId_DeletesCategory()
    {
        var serviceProvider = _fixture.WebAppFactory.Services;


        var elasticClient = _fixture.ElasticClient;

        var categoriesExample = _fixture.GetCategoryModelList();

        await elasticClient.IndexManyAsync(categoriesExample);

        var id =  categoriesExample[3].Id;

        var output = await _fixture
            .GraphQLClient
            .DeleteCategory
            .ExecuteAsync(id,CancellationToken.None);


        output.Data.Should().NotBeNull();
        output.Data!.DeleteCategory.Should().BeTrue();
       

       var deletedCategory = await elasticClient.GetAsync<CategoryModel>(id);

       deletedCategory.Found.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(DeleteCategoryWhenReceivesAndNonExistingId_ReturnsErrors))]
    [Trait("E2E/GraphQL", "[Category] Save")]
    public async Task DeleteCategoryWhenReceivesAndNonExistingId_ReturnsErrors()
    {   
        var elasticClient = _fixture.ElasticClient;

        var categoriesExample = _fixture.GetCategoryModelList();

        await elasticClient.IndexManyAsync(categoriesExample);

        var id = Guid.NewGuid();
        var expectedErrorMessage = $"Category '{id}' not found.";

        var output = await _fixture.GraphQLClient
            .DeleteCategory
            .ExecuteAsync(id, CancellationToken.None);
        
        output.Data.Should().BeNull();

        output.Errors.Should().NotBeNull();

        output.Errors.Single().Message.Should().Be(expectedErrorMessage);
    }



    public void Dispose()
        => _fixture.DeleteAll();
}
