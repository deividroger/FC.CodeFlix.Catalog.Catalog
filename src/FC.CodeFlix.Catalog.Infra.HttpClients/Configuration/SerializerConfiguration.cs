using System.Text.Json;

namespace FC.CodeFlix.Catalog.Infra.HttpClients.Configuration;

public class SerializerConfiguration
{
    public static readonly JsonSerializerOptions SnakeCaseSerializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNamingPolicy = new JsonSnakeCasePolicy()
    };
}