using FC.Codeflix.Catalog.E2ETests;
using FC.CodeFlix.Catalog.Infra.ES;
using FluentAssertions;
using Nest;
using RespositoriesDTOs = FC.CodeFlix.Catalog.Domain.Repositories.DTOs;

namespace FC.CodeFlix.Catalog.E2ETests.Graphql.Genres.SearchGenre;

[Collection(nameof(SearchGenreTestFixture))]
public class SearchGenreTest: IDisposable
{
    private readonly SearchGenreTestFixture _fixture;

    public SearchGenreTest(SearchGenreTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory(DisplayName = nameof(SearchGenreWhenReceivesValidSearchInput_ReturnFilteredList))]
    [Trait("E2E/GraphQL", "[Genre] Search")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 3, 3)]
    [InlineData("Horror", 2, 5, 0, 3)]
    [InlineData("Sci-fi", 1, 5, 4, 4)]
    [InlineData("Sci-fi", 1, 2, 2, 4)]
    [InlineData("Sci-fi", 2, 3, 1, 4)]
    [InlineData("Others", 1, 5, 0, 0)]
    [InlineData("Robots", 1, 5, 2, 2)]
    public async Task SearchGenreWhenReceivesValidSearchInput_ReturnFilteredList(
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

        var examples = _fixture.GetGenreModelList(categoryNameList);

        await elasticClient.IndexManyAsync(examples);


        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.Genre);

        var output = await _fixture
            .GraphQLClient
            .SearchGenre
            .ExecuteAsync(
                page,
                perPage,
                search,
                string.Empty,
                SearchOrder.Asc,
                CancellationToken.None);

        output.Data!.Genres.Should().NotBeNull();


        output.Data.Genres.Items.Should().NotBeNull();
        output.Data.Genres.CurrentPage.Should().Be(page);
        output.Data.Genres.PerPage.Should().Be(perPage);
        output.Data.Genres.Items.Should().HaveCount(expectedItemsCount);
        output.Data.Genres.Total.Should().Be(expectedTotalCount);

        foreach (var outputItem in output.Data.Genres.Items)
        {
            var expected = examples.First(x => x.Id == outputItem.Id);

            outputItem.Name.Should().Be(expected.Name);
            outputItem.IsActive.Should().Be(expected.IsActive);
            outputItem.CreatedAt.Date.Should().Be(expected.CreatedAt.Date);
            outputItem.Categories.Should().BeEquivalentTo(expected.Categories);

        }

    }


    [Theory(DisplayName = nameof(SearchGenreWhenReceivesValidSearchInput_ReturnOrderedList))]
    [Trait("E2E/GraphQL", "[Genre] Search")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("createdat", "asc")]
    [InlineData("createdat", "desc")]
    [InlineData("", "desc")]
    public async Task SearchGenreWhenReceivesValidSearchInput_ReturnOrderedList(string orderBy, string direction)
    {

        var elasticClient = _fixture.ElasticClient;

        var examples = _fixture.GetGenreModelList();

        await elasticClient.IndexManyAsync(examples);
        await elasticClient.Indices.RefreshAsync(ElasticsearchIndices.Genre);

        var page = 1;
        var perPage = examples.Count;
        var order = direction == "asc" ? SearchOrder.Asc : SearchOrder.Desc;


        var output = await _fixture
               .GraphQLClient
               .SearchGenre
               .ExecuteAsync(page, perPage, string.Empty, orderBy, order, CancellationToken.None);


        var expectedList = _fixture
            .CloneGenreListOrdered(
            examples,
            orderBy, order == SearchOrder.Asc ? RespositoriesDTOs.SearchOrder.ASC : RespositoriesDTOs.SearchOrder.DESC);


        output.Data!.Genres.Should().NotBeNull();

        output.Data!.Genres.Items.Should().NotBeNull();
        output.Data!.Genres.CurrentPage.Should().Be(page);
        output.Data!.Genres.PerPage.Should().Be(perPage);
        output.Data!.Genres.Items.Should().HaveCount(examples.Count);
        output.Data!.Genres.Total.Should().Be(examples.Count);


        for (int i = 0; i < output.Data!.Genres.Items.Count; i++)
        {
            var outputItem = output.Data!.Genres.Items[i];
            var expected = expectedList[i];

            outputItem.Id.Should().Be(expected.Id);
            outputItem.Name.Should().Be(expected.Name);
            
            outputItem.IsActive.Should().Be(expected.IsActive);
            outputItem.CreatedAt.Date.Should().Be(expected.CreatedAt.Date);

            outputItem.Categories.Should().BeEquivalentTo(expected.Categories);
        }
    }
    public void Dispose()
        => _fixture.DeleteAll();
}
