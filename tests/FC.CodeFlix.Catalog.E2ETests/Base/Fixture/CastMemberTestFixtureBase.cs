using FC.CodeFlix.Catalog.Infra.ES.Models;
using FC.CodeFlix.Catalog.Tests.Shared;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.CodeFlix.Catalog.E2ETests.Base.Fixture;

public class CastMemberTestFixtureBase : FixtureBase, IDisposable
{
    public CustomerWebApplicationFactory<Program> WebAppFactory { get; private set; } = null!;


    public IElasticClient ElasticClient { get; private set; }

    public CastMemberDataGenerator DataGenerator { get; private set; }

    protected CastMemberTestFixtureBase()
    {
        DataGenerator = new CastMemberDataGenerator();

        WebAppFactory = new CustomerWebApplicationFactory<Program>();

        _ = WebAppFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri(WebAppFactory.BaseUrl)
        });

        ElasticClient = WebAppFactory.Services.GetRequiredService<IElasticClient>();

        ElasticClient.CreateCastMemberIndexAsync().GetAwaiter().GetResult();
    }

    public IList<CastMemberModel> GetCastMemberModelList(int count = 10)
        => DataGenerator.GetCastMemberModelList(count);

    public void DeleteAll()
        => ElasticClient.DeleteDocuments<CastMemberModel>();

    public void Dispose()
        => ElasticClient.DeleteCastMemberIndex();
}
