using Bogus;
using FC.CodeFlix.Catalog.Application;
using FC.CodeFlix.Catalog.Infra.ES;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FC.CodeFlix.Catalog.Integration.Tests.Common;

public abstract class BaseFixture
{

    public IServiceProvider ServiceProvider { get; }

    protected BaseFixture()
    {
        Faker = new Faker("en");
        ServiceProvider = BuildServiceProvider();
    }

    private static IServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();
        var inMemorySerttings = new Dictionary<string, string>()
        {
            {"ConnectionStrings:ElasticSearch","http://localhost:9200" }
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySerttings!)
            .Build();

        services.AddUseCases();
        services.AddElasticSearch(configuration);
        services.AddRepositories();

        return services.BuildServiceProvider();
    }

    public Faker Faker { get; set; }

    public bool GetRandomBoolean()
        => new Random().NextDouble() < 0.5;
}