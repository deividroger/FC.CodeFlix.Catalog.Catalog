using FC.CodeFlix.Catalog.Infra.HttpClients.Extensions;
using System.Text.Json;

namespace FC.CodeFlix.Catalog.Infra.HttpClients.Configuration;

public class JsonSnakeCasePolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
        => name.ToSnakeCase();
}
