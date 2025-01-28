using FC.CodeFlix.Catalog.Infra.Messaging.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace FC.CodeFlix.Catalog.Infra.Messaging.Extensions;

public static class ServiceCollectionExtensions
{
    public static KafkaConsumerBuilder<TMessage> AddKafkaConsumer<TMessage>(this IServiceCollection services)
        where TMessage : class
    {
        return new KafkaConsumerBuilder<TMessage>(services);
    }
}
