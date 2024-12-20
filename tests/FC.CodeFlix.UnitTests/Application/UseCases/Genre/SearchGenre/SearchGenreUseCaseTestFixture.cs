using FC.CodeFlix.Catalog.Domain.Repositories.DTOs;
using FC.CodeFlix.Catalog.UnitTests.Application.UseCases.Genre.Common;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;
using UseCase = FC.CodeFlix.Catalog.Application.UseCases.Genre.SearchGenre;
namespace FC.CodeFlix.Catalog.UnitTests.Application.UseCases.Genre.SearchGenre;

public class SearchGenreUseCaseTestFixture: GenreUseCaseTestFixture
{
    public UseCase.SearchGenreInput GetSearchInput()
    {
        var random = new Random();

        return new UseCase.SearchGenreInput(
            page: random.Next(1, 10),
            perPage: random.Next(10, 20),
            search: DataGenerator.Faker.Commerce.ProductName(),
            orderBy: DataGenerator.Faker.Commerce.ProductName(),
            order: random.Next(0, 2) == 0 ? SearchOrder.ASC : SearchOrder.DESC
            );
    }

    public List<DomainEntity.Genre> GetGenreList(int length = 10)
        => Enumerable
            .Range(0, length)
            .Select(_=> GetValidGenre())
            .ToList();

}


[CollectionDefinition(nameof(SearchGenreUseCaseTestFixture))]
public class SearchGenreUseCaseTestFixtureCollection : ICollectionFixture<SearchGenreUseCaseTestFixture> { }