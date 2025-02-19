using FC.CodeFlix.Catalog.Domain.Gateways;
using FC.CodeFlix.Catalog.Infra.HttpClients.DelegatingHandlers;
using FC.CodeFlix.Catalog.Infra.HttpClients.HttpClients;
using FC.CodeFlix.Catalog.Infra.HttpClients.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace FC.CodeFlix.Catalog.Infra.HttpClients;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<CredentialsModel>()
            .BindConfiguration("HttpClients:AuthenticationServer:Credentials");

        services
            .AddHttpClient<AuthenticationClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["HttpClients:AuthenticationServer:BaseUrl"]!);
            })
            .AddPolicyHandler(GetRetryPolicy(5));

        services
            .AddScoped<AuthenticationHandler>()
            .AddHttpClient<IAdminCatalogGateway, AdminCatalogClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["HttpClients:AdminCatalogBaseUrl"]!);
            })
            .AddPolicyHandler(GetRetryPolicy(2))
            .AddHttpMessageHandler<AuthenticationHandler>(); ;

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount)
        => HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}