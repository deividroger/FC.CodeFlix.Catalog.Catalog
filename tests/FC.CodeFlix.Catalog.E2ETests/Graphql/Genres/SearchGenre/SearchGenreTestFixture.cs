using FC.CodeFlix.Catalog.E2ETests.Graphql.Genres.Common;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using RepositoryDTOs = FC.CodeFlix.Catalog.Domain.Repositories.DTOs;

namespace FC.CodeFlix.Catalog.E2ETests.Graphql.Genres.SearchGenre;


public class SearchGenreTestFixture : GenreTestFixture
{
    public IList<GenreModel> GetGenreModelList(List<string> names)
        => DataGenerator.GetGenreModelList(names);
    public IList<GenreModel> CloneGenreListOrdered(IList<GenreModel> genresList, string orderBy, RepositoryDTOs.SearchOrder direction)
        => DataGenerator.CloneGenreListOrdered(genresList, orderBy, direction);
}

[CollectionDefinition(nameof(SearchGenreTestFixture))]
public class SearchGenreTestFixtureCollection : ICollectionFixture<SearchGenreTestFixture>
{

}