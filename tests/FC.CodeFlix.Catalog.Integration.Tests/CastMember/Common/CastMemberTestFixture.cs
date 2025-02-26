using FC.CodeFlix.Catalog.Infra.ES.Models;
using FC.CodeFlix.Catalog.Integration.Tests.Common;
using FC.CodeFlix.Catalog.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.CodeFlix.Catalog.Integration.Tests.CastMember.Common;


public class CastMemberTestFixture : BaseFixture, IDisposable
{
    public CastMemberDataGenerator DataGenerator { get; }

    public IElasticClient ElasticClient { get; }

    public CastMemberTestFixture() : base()
    {
        DataGenerator = new CastMemberDataGenerator();
        ElasticClient = ServiceProvider.GetRequiredService<IElasticClient>();

        ElasticClient.CreateCastMemberIndexAsync().GetAwaiter().GetResult();
    }

    public IList<CastMemberModel> GetCastMemberModelList(int count = 10)
        => DataGenerator.GetCastMemberModelList(count);

    public void DeleteAll()
        => ElasticClient.DeleteDocuments<CastMemberModel>();

    public void Dispose()
        => ElasticClient.DeleteCastMemberIndex();
}

[CollectionDefinition(nameof(CastMemberTestFixture))]
public class CastMemberTestFixtureCollection : ICollectionFixture<CastMemberTestFixture>
{ }
