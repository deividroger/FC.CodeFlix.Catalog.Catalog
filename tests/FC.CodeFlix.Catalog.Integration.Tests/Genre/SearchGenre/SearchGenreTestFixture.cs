using FC.CodeFlix.Catalog.Domain.Repositories.DTOs;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using FC.CodeFlix.Catalog.Integration.Tests.Genre.Common;

namespace FC.CodeFlix.Catalog.Integration.Tests.Genre.SearchGenre;

public class SearchGenreTestFixture: GenreTestFixture
{
    public IList<GenreModel> GetGenreModelList(List<string> genreName)
     => DataGenerator.GetGenreModelList(genreName);

    public IList<GenreModel> CloneGenresListOrdered(
        IList<GenreModel> genresList,
        string orderBy,
        SearchOrder direction)
    => DataGenerator.CloneGenreListOrdered(genresList, orderBy, direction);
}


[CollectionDefinition(nameof(SearchGenreTestFixture))]
public class SearchGenreTestFixtureCollection : ICollectionFixture<SearchGenreTestFixture> { }
