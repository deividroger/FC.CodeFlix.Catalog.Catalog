using System.Text.Json.Serialization;

namespace FC.CodeFlix.Catalog.Infra.HttpClients.Models;

public class AuthenticationResponseModel
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresInSeconds { get; set; }
}
