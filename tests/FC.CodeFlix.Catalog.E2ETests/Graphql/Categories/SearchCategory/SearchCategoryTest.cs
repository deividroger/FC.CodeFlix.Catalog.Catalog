using FC.Codeflix.Catalog.E2ETests;
using FC.CodeFlix.Catalog.Infra.ES;
using FluentAssertions;
using Nest;
using RespositoriesDTOs = FC.CodeFlix.Catalog.Domain.Repositories.DTOs;


namespace FC.CodeFlix.Catalog.E2ETests.Graphql.Categories.SearchCategory;

[Collection(nameof(SearchCategoryTestFixture))]
public class SearchCategoryTest : IDisposable
{
    private readonly SearchCategoryTestFixture _fixture;

    public SearchCategoryTest(SearchCategoryTestFixture fixture)
        => this._fixture = fixture;


    [Theory(DisplayName = nameof(SearchCategoryWhenReceivesValidSearchInput_ReturnFilteredList))]
    [Trait("E2E/GraphQL", "[Category] Search")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 3, 3)]
    [InlineData("Horror", 2, 5, 0, 3)]
    [InlineData("Sci-fi", 1, 5, 4, 4)]
    [InlineData("Sci-fi", 1, 2, 2, 4)]
    [InlineData("Sci-fi", 2, 3, 1, 4)]
    [InlineData("Others", 1, 5, 0, 0)]
    [InlineData("Robots", 1, 5, 2, 2)]
    public async Task SearchCategoryWhenReceivesValidSearchInput_ReturnFilteredList(
       string search,
       int page,
       int perPage,
       int expectedItemsCount,
       int expectedTotalCount
       )
    {

        var elasticClient = _fixture.ElasticClient;

        var categoryNameList = new List<string>()
        {
            "Action",
            "Horror",
            "Horror - Robots",
            "Horror - Based on Real Facts",
            "Drama",
            "Sci-fi IA",
            "Sci-fi Space",
            "Sci-fi Robots",
            "Sci-fi Future"
        };

        var examples = _fixture.GetCategoryModelList(categoryNameList);

        await elasticClient.IndexManyAsync(examples);


        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.Category);

        var output = await _fixture
            .GraphQLClient
            .SearchCategory
            .ExecuteAsync(
                page,
                perPage,
                search,
                string.Empty,
                SearchOrder.Asc,
                CancellationToken.None);

        output.Data!.Categories.Should().NotBeNull();


        output.Data.Categories.Items.Should().NotBeNull();
        output.Data.Categories.CurrentPage.Should().Be(page);
        output.Data.Categories.PerPage.Should().Be(perPage);
        output.Data.Categories.Items.Should().HaveCount(expectedItemsCount);
        output.Data.Categories.Total.Should().Be(expectedTotalCount);

        foreach (var outputItem in output.Data.Categories.Items)
        {
            var expected = examples.First(x => x.Id == outputItem.Id);

            outputItem.Name.Should().Be(expected.Name);
            outputItem.Description.Should().Be(expected.Description);
            outputItem.IsActive.Should().Be(expected.IsActive);
            outputItem.CreatedAt.Should().Be(expected.CreatedAt);

        }

    }


    [Theory(DisplayName = nameof(SearchCategoryWhenReceivesValidSearchInput_ReturnOrderedList))]
    [Trait("E2E/GraphQL", "[Category] Search")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("createdat", "asc")]
    [InlineData("createdat", "desc")]
    [InlineData("", "desc")]
    public async Task SearchCategoryWhenReceivesValidSearchInput_ReturnOrderedList(string orderBy, string direction)
    {

        var elasticClient = _fixture.ElasticClient;

        var examples = _fixture.GetCategoryModelList();

        await elasticClient.IndexManyAsync(examples);
        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.Category);

        var page = 1;
        var perPage = examples.Count;
        var order = direction == "asc" ? SearchOrder.Asc : SearchOrder.Desc;


        var output = await _fixture
               .GraphQLClient
               .SearchCategory
               .ExecuteAsync(page, perPage, string.Empty, orderBy, order, CancellationToken.None);


        var expectedList = _fixture
            .CloneCategoriesListOrdered(
            examples,
            orderBy, order == SearchOrder.Asc ? RespositoriesDTOs.SearchOrder.ASC : RespositoriesDTOs.SearchOrder.DESC);


        output.Data!.Categories.Should().NotBeNull();

        output.Data!.Categories.Items.Should().NotBeNull();
        output.Data!.Categories.CurrentPage.Should().Be(page);
        output.Data!.Categories.PerPage.Should().Be(perPage);
        output.Data!.Categories.Items.Should().HaveCount(examples.Count);
        output.Data!.Categories.Total.Should().Be(examples.Count);


        for (int i = 0; i < output.Data!.Categories.Items.Count; i++)
        {
            var outputItem = output.Data!.Categories.Items[i];
            var expected = expectedList[i];

            outputItem.Id.Should().Be(expected.Id);
            outputItem.Name.Should().Be(expected.Name);
            outputItem.Description.Should().Be(expected.Description);
            outputItem.IsActive.Should().Be(expected.IsActive);
            outputItem.CreatedAt.Should().Be(expected.CreatedAt);

        }
    }


    public void Dispose()
        => _fixture.DeleteAll();
}
