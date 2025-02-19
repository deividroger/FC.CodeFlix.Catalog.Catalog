using Newtonsoft.Json.Serialization;

namespace FC.CodeFlix.Catalog.Infra.HttpClients.Extensions;

public static class StringSnakeCaseExtensions
{
    private readonly static NamingStrategy _snakeCaseNamingStrategy = new SnakeCaseNamingStrategy();

    public static string ToSnakeCase(this string stringToConvert)
    {
        ArgumentNullException
            .ThrowIfNullOrEmpty(stringToConvert, nameof(stringToConvert));

        return _snakeCaseNamingStrategy.GetPropertyName(stringToConvert,false);
    }

}
