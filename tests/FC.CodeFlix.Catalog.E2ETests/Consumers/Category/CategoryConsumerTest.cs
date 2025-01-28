using FC.CodeFlix.Catalog.E2ETests.Graphql.Categories.Common;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using FluentAssertions;
using Nest;

namespace FC.CodeFlix.Catalog.E2ETests.Consumers.Category;


[Collection(nameof(CategoryConsumerTestFixture))]
public class CategoryConsumerTest : IDisposable
{
    private readonly CategoryConsumerTestFixture _fixture;

    public CategoryConsumerTest(CategoryConsumerTestFixture fixture)
    {
        _fixture = fixture;
    }


    [Fact(DisplayName = nameof(CategoryEvent_WhenOperationIsCreate_SavesCategory))]
    [Trait("E2E/Consumer", "Category")]
    public async Task CategoryEvent_WhenOperationIsCreate_SavesCategory()
    {
        var message = _fixture.BuildValidMessage("c");

        var category = message.Payload.After;

        await _fixture.PublishMessageAsync(message);
        await Task.Delay(2_000);

        var persisted = await  _fixture.ElasticClient.GetAsync<CategoryModel>(category.Id);

        persisted.Found.Should().BeTrue();

        var document = persisted.Source;
        document.Should().NotBeNull();

        document.Id.Should().Be(category.Id);
        document.Name.Should().Be(category.Name);
        document.Description.Should().Be(category.Description);
        document.IsActive.Should().Be(category.IsActive);
        document.CreatedAt.Date.Should().Be(category.CreatedAt.Date);

    }

    [Fact(DisplayName = nameof(CategoryEvent_WhenOperationIsUpdate_SavesCategory))]
    [Trait("E2E/Consumer", "Category")]
    public async Task CategoryEvent_WhenOperationIsUpdate_SavesCategory()
    {
        var examplesList = _fixture.GetCategoryModelList();


        var example = examplesList[2];

        await _fixture.ElasticClient.IndexManyAsync(examplesList);

        var message = _fixture.BuildValidMessage("u",example);

        var category = message.Payload.After;

        category.Name = _fixture.DataGenerator.GetValidCategoryName();

        await _fixture.PublishMessageAsync(message);
        await Task.Delay(2_000);

        var persisted = await _fixture.ElasticClient.GetAsync<CategoryModel>(category.Id);

        persisted.Found.Should().BeTrue();

        var document = persisted.Source;
        document.Should().NotBeNull();

        document.Id.Should().Be(category.Id);
        document.Name.Should().Be(category.Name);
        document.Description.Should().Be(category.Description);
        document.IsActive.Should().Be(category.IsActive);
        document.CreatedAt.Date.Should().Be(category.CreatedAt.Date);

    }

    [Fact(DisplayName = nameof(CategoryEvent_WhenOperationIsDelete_DeleteCategory))]
    [Trait("E2E/Consumer", "Category")]
    public async Task CategoryEvent_WhenOperationIsDelete_DeleteCategory()
    {
        var examplesList = _fixture.GetCategoryModelList();

        var example = examplesList[2];

        await _fixture.ElasticClient.IndexManyAsync(examplesList);

        var message = _fixture.BuildValidMessage("d", example);

        var category = message.Payload.Before;

        await _fixture.PublishMessageAsync(message);
        await Task.Delay(2_000);

        var persisted = await _fixture.ElasticClient.GetAsync<CategoryModel>(category.Id);

        persisted.Found.Should().BeFalse();

    }


    [Fact(DisplayName = nameof(CategoryEvent_WhenOperationIsRead_SavesCategory))]
    [Trait("E2E/Consumer", "Category")]
    public async Task CategoryEvent_WhenOperationIsRead_SavesCategory()
    {
        var message = _fixture.BuildValidMessage("r");

        var category = message.Payload.After;

        await _fixture.PublishMessageAsync(message);
        await Task.Delay(2_000);

        var persisted = await _fixture.ElasticClient.GetAsync<CategoryModel>(category.Id);

        persisted.Found.Should().BeTrue();

        var document = persisted.Source;
        document.Should().NotBeNull();

        document.Id.Should().Be(category.Id);
        document.Name.Should().Be(category.Name);
        document.Description.Should().Be(category.Description);
        document.IsActive.Should().Be(category.IsActive);
        document.CreatedAt.Date.Should().Be(category.CreatedAt.Date);

    }

    public void Dispose()
        => _fixture.DeleteAll();
}
