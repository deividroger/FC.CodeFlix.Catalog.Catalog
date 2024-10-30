using Microsoft.Extensions.DependencyInjection;

namespace FC.CodeFlix.Catalog.Application;

public static class ServiceRegistrationExtensions
{
    public static  IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddMediatR(x=> x.RegisterServicesFromAssemblies(typeof(ServiceRegistrationExtensions).Assembly));

        return services;
    }
}
