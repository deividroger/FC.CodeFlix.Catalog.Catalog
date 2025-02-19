using FC.CodeFlix.Catalog.Infra.HttpClients.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace FC.CodeFlix.Catalog.Infra.HttpClients.HttpClients;

internal class AuthenticationClient
{
    private readonly HttpClient _client;

    private readonly CredentialsModel _credentialsModel;

    public AuthenticationClient(HttpClient client, IOptions<CredentialsModel> options)
    {
        _client = client;
        _credentialsModel = options.Value;
    }

    public async Task<AuthenticationResponseModel> GetAccessToken(string username, string password, CancellationToken cancellationToken = default)
    {   
        var request = new HttpRequestMessage(HttpMethod.Post, "/realms/fc3-codeflix/protocol/openid-connect/token");
        var collection = new List<KeyValuePair<string, string>>
        {
            new("client_id", _credentialsModel.ClientId),
            new("client_secret", _credentialsModel.ClientSecret),
            new("grant_type", "password"),
            new("username", username),
            new("password", password)
        };

        var content = new FormUrlEncodedContent(collection);
        request.Content = content;
        var response = await _client.SendAsync(request,cancellationToken);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<AuthenticationResponseModel>(cancellationToken: cancellationToken))!;

    }
}
