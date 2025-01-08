using FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.Domain.Exceptions;
using FC.CodeFlix.Catalog.Domain.Repositories;
using FC.CodeFlix.Catalog.Domain.Repositories.DTOs;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using Nest;
using SearchInput = FC.CodeFlix.Catalog.Domain.Repositories.DTOs.SearchInput;

namespace FC.CodeFlix.Catalog.Infra.ES.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly IElasticClient _elasticClient;

    public GenreRepository(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _elasticClient.DeleteAsync<GenreModel>(id, ct: cancellationToken);

        if (response.Result == Result.NotFound)
        {
            throw new NotFoundException($"Genre '{id}' not found.");
        }
    }

    public async Task SaveAsync(Genre entity, CancellationToken cancellationToken)
    {
        var model = GenreModel.FromEntity(entity);

        await _elasticClient.IndexDocumentAsync<GenreModel>(model, cancellationToken);
    }

    public async Task<SearchOutput<Genre>> SearchAsync(SearchInput input, CancellationToken cancellationToken)
    {
        var response = await _elasticClient.SearchAsync<GenreModel>(s => s
              .Query(q => q
                   .Match(m => m
                          .Field(f => f.Name)
                          .Query(input.Search)
                   )
               )
               .From(input.From)
               .Size(input.PerPage)
               .Sort(BuildSortExpression(input.OrderBy, input.Order))
       , ct: cancellationToken);

        var genres = response.Documents
            .Select(doc => doc.ToEntity())
            .ToList();

        return new SearchOutput<Genre>(input.Page, input.PerPage, (int)response.Total, genres);
    }

    private static Func<SortDescriptor<GenreModel>, IPromise<IList<ISort>>> BuildSortExpression(string orderBy, SearchOrder order)
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
