using FC.CodeFlix.Catalog.Infra.Messaging.JsonConverters;
using System.Text.Json;

namespace FC.CodeFlix.Catalog.Infra.Messaging.Configuration;

public static class SerializerConfiguration
{
    public static JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new BoolConverter(), new DateTimeConverter() }
    };
}
