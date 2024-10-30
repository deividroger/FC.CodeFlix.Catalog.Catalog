using FC.Codeflix.Catalog.E2ETests;
using FC.CodeFlix.Catalog.E2ETests.Graphql.Categories.Common;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.CodeFlix.Catalog.E2ETests.Graphql.Categories.SaveCategory;

[Collection(nameof(CategoryTestFixture))]
public class SaveCategoryTest : IDisposable
{
    private readonly CategoryTestFixture _fixture;

    public SaveCategoryTest(CategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(SaveCategory_When_InputIsValid_Persists_Category))]
    [Trait("E2E/GraphQL", "[Category] Save")]
    public async Task SaveCategory_When_InputIsValid_Persists_Category()
    {
        var serviceProvider = _fixture.WebAppFactory.Services;
        
        var elasticClient = serviceProvider.GetRequiredService<IElasticClient>();

        var input = new SaveCategoryInput() { 
                Id = Guid.NewGuid(), 
                Name="Action",
                Description = "Action Test", 
                CreatedAt = DateTime.UtcNow.Date, 
                IsActive =true 
        };

        var output = await _fixture.GraphQLClient.SaveCategory
            .ExecuteAsync(input,CancellationToken.None);

        var persisted = await elasticClient.GetAsync<CategoryModel>(input.Id);


        persisted.Found.Should().BeTrue();

        var document = persisted.Source;
        document.Should().NotBeNull();

        document.Id.Should().Be(input.Id);
        document.Name.Should().Be(input.Name);
        document.Description.Should().Be(input.Description);
        document.IsActive.Should().Be(input.IsActive);
        document.CreatedAt.Should().Be(input.CreatedAt.DateTime);

        output.Data!.SaveCategory.Should().NotBeNull();
            
        output.Data.SaveCategory.Id.Should().Be(input.Id);
        output.Data.SaveCategory.Name.Should().Be(input.Name);
        output.Data.SaveCategory.Description.Should().Be(input.Description);
        output.Data.SaveCategory.IsActive.Should().Be(input.IsActive);
        output.Data.SaveCategory.CreatedAt.Should().Be(input.CreatedAt);


    }

    [Fact(DisplayName = nameof(SaveCategory_When_InputIsInValid_ThrowsException))]
    [Trait("E2E/GraphQL", "[Category] Save")]
    public async Task SaveCategory_When_InputIsInValid_ThrowsException()
    {
        var serviceProvider = _fixture.WebAppFactory.Services ;
        
        var elasticClient = serviceProvider.GetRequiredService<IElasticClient>();

        var expectedMessage = "Name should not be empty or null";

        var input = new SaveCategoryInput()
        {
            Id = Guid.NewGuid(),
            Name =string.Empty,
            Description = "Action Test",
            CreatedAt = DateTime.UtcNow.Date,
            IsActive = true
        };

        var output = await _fixture.GraphQLClient.SaveCategory
            .ExecuteAsync(input, CancellationToken.None);

        output.Data.Should().BeNull();

        output.Errors.Should().NotBeEmpty();

        output.Errors.Single().Message.Should().Be(expectedMessage);

        var persisted = await elasticClient.GetAsync<CategoryModel>(input.Id);

        persisted.Found.Should().BeFalse();

    }

    public void Dispose() => _fixture.DeleteAll();
}
