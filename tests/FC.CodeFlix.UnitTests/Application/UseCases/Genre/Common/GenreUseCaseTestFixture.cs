using FC.CodeFlix.Catalog.Domain.Gateways;
using FC.CodeFlix.Catalog.Domain.Repositories;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using FC.CodeFlix.Catalog.Tests.Shared;
using NSubstitute;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;


namespace FC.CodeFlix.Catalog.UnitTests.Application.UseCases.Genre.Common;

public class GenreUseCaseTestFixture
{
    public GenreDataGenerator DataGenerator { get; }

    public GenreUseCaseTestFixture()
        => DataGenerator = new GenreDataGenerator();

    public IGenreRepository GetMockRepository()
        => Substitute.For<IGenreRepository>();

    public IAdminCatalogGateway GetMockAdminCatalogGateway()
        => Substitute.For<IAdminCatalogGateway>();

    public DomainEntity.Genre GetValidGenre()
        => DataGenerator.GetValidGenre();

    public IList<DomainEntity.Genre> GetGenreList(int count = 10)
        => DataGenerator.GetGenreList(count);
}

[CollectionDefinition(nameof(GenreUseCaseTestFixture))]
public class GenreUseCaseFixtureCollection : ICollectionFixture<GenreUseCaseTestFixture> { }