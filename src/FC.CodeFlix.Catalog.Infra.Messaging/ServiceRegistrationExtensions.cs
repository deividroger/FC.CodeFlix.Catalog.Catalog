﻿using FC.CodeFlix.Catalog.Infra.Messaging.Configuration;
using FC.CodeFlix.Catalog.Infra.Messaging.Consumers.MessageHandlers.CastMember;
using FC.CodeFlix.Catalog.Infra.Messaging.Consumers.MessageHandlers.Category;
using FC.CodeFlix.Catalog.Infra.Messaging.Consumers.MessageHandlers.Genre;
using FC.CodeFlix.Catalog.Infra.Messaging.Consumers.MessageHandlers.Video;
using FC.CodeFlix.Catalog.Infra.Messaging.Extensions;
using FC.CodeFlix.Catalog.Infra.Messaging.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FC.CodeFlix.Catalog.Infra.Messaging;

public static class ServiceRegistrationExtensions
{
    private const string KafkaConfigurationSection = "KafkaConfiguration";
    public static IServiceCollection AddConsumers(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<KafkaConfiguration>()
            .BindConfiguration(KafkaConfigurationSection);

        var kafkaConfiguration = configuration.GetSection(KafkaConfigurationSection);

        return services
            .AddCategoryConsumers(kafkaConfiguration.GetSection(nameof(KafkaConfiguration.CategoryConsumer)))
            .AddGenreConsumers(
                kafkaConfiguration.GetSection(nameof(KafkaConfiguration.GenreConsumer)),
                kafkaConfiguration.GetSection(nameof(KafkaConfiguration.GenreCategoryConsumer)))
            .AddCastMemberConsumers(kafkaConfiguration.GetSection(nameof(KafkaConfiguration.CastMemberConsumer)))
            .AddVideoConsumers(kafkaConfiguration.GetSection(nameof(KafkaConfiguration.VideoConsumer)));
    }
    private static IServiceCollection AddCategoryConsumers(
        this IServiceCollection services,
        IConfigurationSection configurationSection)
    {
        return services
            .AddScoped<SaveCategoryMessageHandler>()
            .AddScoped<DeleteCategoryMessageHandler>()
            .AddKafkaConsumer<CategoryPayloadModel>()
            .Configure(configurationSection)
            .WithRetries()
            .With<SaveCategoryMessageHandler>()
            .When(message => message.Payload.Operation is
                MessageModelOperation.Create or
                MessageModelOperation.Read or
                MessageModelOperation.Update)
            .And
            .With<DeleteCategoryMessageHandler>()
            .When(message => message.Payload.Operation is MessageModelOperation.Delete)
            .Register();
    }

    private static IServiceCollection AddGenreConsumers(
         this IServiceCollection services,
         IConfigurationSection genreConfigurationSection,
         IConfigurationSection genreCategoryConfigurationSection)
    {
        return services
            .AddScoped(typeof(SaveGenreMessageHandler<>))
            .AddScoped<DeleteGenreMessageHandler>()
            .AddKafkaConsumer<GenrePayloadModel>()
            .Configure(genreConfigurationSection)
            .WithRetries()
            .With<SaveGenreMessageHandler<GenrePayloadModel>>()
            .When(message => message.Payload.Operation is
                MessageModelOperation.Create or
                MessageModelOperation.Read or
                MessageModelOperation.Update)
            .And
            .With<DeleteGenreMessageHandler>()
            .When(message => message.Payload.Operation is MessageModelOperation.Delete)
            .Register()
            .AddKafkaConsumer<GenreCategoryPayloadModel>()
            .Configure(genreCategoryConfigurationSection)
            .WithRetries()
            .WithDefault<SaveGenreMessageHandler<GenreCategoryPayloadModel>>()
            .Register();
    }

    private static IServiceCollection AddCastMemberConsumers(this IServiceCollection services, IConfigurationSection configurationSection)
    {
        return services
            .AddScoped<SaveCastMemberMessageHandler>()
            .AddScoped<DeleteCastMemberMessageHandler>()
            .AddKafkaConsumer<CastMemberPayloadModel>()
                   .Configure(configurationSection)
                   .WithRetries(3)
                   .With<SaveCastMemberMessageHandler>()
                   .When(message => message.Payload.Operation is
                                  MessageModelOperation.Create
                               or MessageModelOperation.Read
                               or MessageModelOperation.Update)
                .And
                    .With<DeleteCastMemberMessageHandler>()
                    .When(message => message.Payload.Operation is MessageModelOperation.Delete)
               .Register();
    }

    private static IServiceCollection AddVideoConsumers(
       this IServiceCollection services,
       IConfigurationSection configurationSection)
    {
        return services
            .AddScoped<SaveVideoMessageHandler>()
            .AddScoped<DeleteVideoMessageHandler>()
            .AddKafkaConsumer<VideoPayloadModel>()
            .Configure(configurationSection)
            .WithRetries()
            .With<SaveVideoMessageHandler>()
            .When(message => message.Payload.Operation is
                MessageModelOperation.Create or
                MessageModelOperation.Read or
                MessageModelOperation.Update)
            .And
            .With<DeleteVideoMessageHandler>()
            .When(message => message.Payload.Operation is MessageModelOperation.Delete)
            .Register();
    }

}