
using FC.CodeFlix.Catalog.Infra.HttpClients.HttpClients;
using FC.CodeFlix.Catalog.Infra.HttpClients.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace FC.CodeFlix.Catalog.Infra.HttpClients.DelegatingHandlers;

internal class AuthenticationHandler : DelegatingHandler
{
    private readonly AuthenticationClient _authClient;
    private readonly CredentialsModel _credentialsModel;
    private readonly IMemoryCache _cache;
    private const string CacheKey = "token";

    public AuthenticationHandler(AuthenticationClient client, IOptions<CredentialsModel> options, IMemoryCache cache)
    {
        _authClient = client;
        _credentialsModel = options.Value;
        _cache = cache;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {

        if (!_cache.TryGetValue("token", out string? token))
        {
            var authResponse = await _authClient.GetAccessToken(_credentialsModel.Username, _credentialsModel.Password, cancellationToken);
            token = authResponse.AccessToken;
            var cacheOption = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(authResponse.ExpiresInSeconds - 5));
            _cache.Set(token, token, cacheOption);
        }

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }
}
