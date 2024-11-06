using FC.CodeFlix.Catalog.Infra.Messaging.Configuration;
using FC.CodeFlix.Catalog.Infra.Messaging.Consumers;
using Microsoft.Extensions.DependencyInjection;

namespace FC.CodeFlix.Catalog.Infra.Messaging;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddConsumers(this IServiceCollection services) {


        services.AddOptions<KafkaConfiguration>()
            .BindConfiguration("KafkaConfiguration");

        return services.AddHostedService<CategoryConsumer>();
    }
} 
