using FC.CodeFlix.Catalog.Domain.Repositories;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using FC.CodeFlix.Catalog.Infra.ES.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FC.CodeFlix.Catalog.Infra.ES;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddElasticSearch(this IServiceCollection  services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ElasticSearch");
        var uri = new Uri(connectionString!);

        var connectionSettings = new ConnectionSettings(uri)
            .DefaultMappingFor<CategoryModel>(i => i
            .IndexName(ElasticsearchIndices.Category)
            .IdProperty(i => i.Id))

            .DefaultMappingFor<GenreModel>(i => i
            .IndexName(ElasticsearchIndices.Genre)
            .IdProperty(i => i.Id))

            .DefaultMappingFor<CastMemberModel>(i => i
            .IndexName(ElasticsearchIndices.CastMember)
            .IdProperty(i => i.Id))

            .DefaultMappingFor<VideoModel>(i => i
            .IndexName(ElasticsearchIndices.Video)
            .IdProperty(i => i.Id))

            //.EnableDebugMode()
            .PrettyJson()
            .ThrowExceptions()
            .RequestTimeout(TimeSpan.FromMinutes(2));

        var client = new ElasticClient(connectionSettings);
        services.AddSingleton<IElasticClient>(client);

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<ICastMemberRepository, CastMemberRepository>();
        services.AddScoped<IVideoRepository, VideoRepository>();

        return services;
    }

}
