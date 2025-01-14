﻿using FC.CodeFlix.Catalog.Infra.ES.Models;
using FC.CodeFlix.Catalog.Integration.Tests.Common;

using FC.CodeFlix.Catalog.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;
using Nest;


namespace FC.CodeFlix.Catalog.Integration.Tests.Genre.Common;

public class GenreTestFixture: BaseFixture, IDisposable
{
    public  IElasticClient ElasticClient { get; }

    public GenreDataGenerator DataGenerator { get; }

    public GenreTestFixture()
    {
        ElasticClient = ServiceProvider.GetRequiredService<IElasticClient>();
        DataGenerator = new GenreDataGenerator();

        ElasticClient.CreateGenreIndexAsync().GetAwaiter().GetResult() ;

    }

    public IList<GenreModel> GetGenreModelList(int count = 10)
        => DataGenerator.GetGenreModelList(count);

    public void DeleteAll()
        => ElasticClient.DeleteDocuments<GenreModel>();

    public void Dispose()
        => ElasticClient.DeleteGenreIndex();
}


[CollectionDefinition(nameof(GenreTestFixture))]
public class GenreTestFixtureCollection : ICollectionFixture<GenreTestFixture>
{ }
