﻿using FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.Domain.Gateways;
using FC.CodeFlix.Catalog.Infra.HttpClients.Configuration;
using FC.CodeFlix.Catalog.Infra.HttpClients.Models;
using System.Net.Http.Json;

namespace FC.CodeFlix.Catalog.Infra.HttpClients.HttpClients;

public class AdminCatalogClient : IAdminCatalogGateway
{
    private readonly HttpClient _client;

    public AdminCatalogClient(HttpClient client) =>
        _client = client;

    public async Task<Genre> GetGenreAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _client.GetFromJsonAsync<DataWrapper<GenreOutputModel>>($"genres/{id}", SerializerConfiguration.SnakeCaseSerializerOptions, cancellationToken);
        return response!.Data.ToGenre();
    }

    public Task<Video> GetVideoAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
