﻿using FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.Domain.Exceptions;
using FC.CodeFlix.Catalog.Domain.Repositories;
using FC.CodeFlix.Catalog.Domain.Repositories.DTOs;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using Nest;
using SearchInput = FC.CodeFlix.Catalog.Domain.Repositories.DTOs.SearchInput;

namespace FC.CodeFlix.Catalog.Infra.ES.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly IElasticClient _client;

    public CategoryRepository(IElasticClient client)
    {
        _client = client;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _client.DeleteAsync<CategoryModel>(id, ct: cancellationToken);

        if(response.Result  == Result.NotFound)
        {
            throw new NotFoundException($"Category '{id}' not found.");
        }
    }

    public async Task SaveAsync(Category entity, CancellationToken cancellationToken)
    {
        var model = CategoryModel.FromEntity(entity);
        await _client.IndexDocumentAsync(model, cancellationToken);
    }

    public async Task<SearchOutput<Category>> SearchAsync(SearchInput input, CancellationToken cancellationToken)
    {
        var response = await _client.SearchAsync<CategoryModel>(s => s
               .Query(q => q
                    .Match(m => m
                           .Field(f => f.Name)
                           .Query(input.Search)
                    )
                )
                .From(input.From)
                .Size(input.PerPage)
                .Sort(BuildSortExpression(input.OrderBy,input.Order))
        , ct: cancellationToken);

        var categories = response.Documents
            .Select(doc => doc.ToEntity())
            .ToList();

        return new SearchOutput<Category>(input.Page, input.PerPage, (int)response.Total, categories);

    }

    private static Func<SortDescriptor<CategoryModel>, IPromise<IList<ISort>>> BuildSortExpression(string orderBy, SearchOrder order)
    => (orderBy.ToLower(), order)
        switch
    {
        ("name", SearchOrder.ASC) => sort => sort
             .Ascending(f => f.Name.Suffix("keyword"))
             .Ascending(f => f.Id),
        ("name", SearchOrder.DESC) => sort => sort
             .Descending(f => f.Name.Suffix("keyword"))
             .Descending(f => f.Id),

        ("id", SearchOrder.ASC) => sort => sort
             .Ascending(f => f.Id),
        ("id", SearchOrder.DESC) => sort => sort
             .Descending(f => f.Id),

        ("createdat", SearchOrder.ASC) => sort => sort
             .Ascending(f => f.CreatedAt),
        ("createdat", SearchOrder.DESC) => sort => sort
             .Descending(f => f.CreatedAt),

        _ => sort => sort
             .Ascending(f => f.Name.Suffix("keyword"))
             .Ascending(f => f.Id)
    }; 
    
}