using FC.CodeFlix.Catalog.Domain.Repositories;
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

    public DomainEntity.Genre GetValidGenre()
        => DataGenerator.GetValidGenre();


}

[CollectionDefinition(nameof(GenreUseCaseTestFixture))]
public class GenreUseCaseFixtureCollection : ICollectionFixture<GenreUseCaseTestFixture> { }