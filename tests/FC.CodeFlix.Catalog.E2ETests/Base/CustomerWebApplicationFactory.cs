using FC.Codeflix.Catalog.E2ETests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace FC.CodeFlix.Catalog.E2ETests.Base;

public class CustomerWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup>
    where TStartup : class
{
    public readonly string BaseUrl = "http://localhost:61000/";
    public CatalogClient GraphqlClient { get; private set; }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var environment = "EndToEndTest";
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);

        builder.UseEnvironment(environment);
        GraphqlClient = BuildGraphqlClient();
        base.ConfigureWebHost(builder);
    }

    private CatalogClient BuildGraphqlClient()
    {
        var services = new ServiceCollection();
        services.AddTransient<HttpMessageHandlerBuilder>(sp => new TestServerHttpMessageHandlerBuilder(Server));
        services.AddCatalogClient()
              .ConfigureHttpClient(client =>
              {
                  client.BaseAddress = new Uri($"{BaseUrl}graphql");
              });
        var provider = services.BuildServiceProvider();

        return provider.GetRequiredService<CatalogClient>();
    }
}
