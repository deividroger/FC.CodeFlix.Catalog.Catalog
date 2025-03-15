using FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.Domain.Exceptions;
using FC.CodeFlix.Catalog.Domain.Repositories;
using FC.CodeFlix.Catalog.Domain.Repositories.DTOs;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using Nest;


namespace FC.CodeFlix.Catalog.Infra.ES.Repositories;

public class VideoRepository : IVideoRepository
{
    private readonly IElasticClient _elasticClient;

    public VideoRepository(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _elasticClient.DeleteAsync<VideoModel>(id, ct: cancellationToken);

        if (response.Result == Result.NotFound)
        {
            throw new NotFoundException($"Video '{id}' not found.");
        }
    }

    public async Task SaveAsync(Video entity, CancellationToken cancellationToken)
    {
        var videoModel = VideoModel.FromEntity(entity);
        await _elasticClient.IndexDocumentAsync(videoModel, ct: cancellationToken);
    }

    public async Task<SearchOutput<Video>> SearchAsync(Domain.Repositories.DTOs.SearchInput input, CancellationToken cancellationToken)
    {
        var response = await _elasticClient.SearchAsync<VideoModel>(s => s
             .Query(q => q
                  .Match(m => m
                         .Field(f => f.Title)
                         .Query(input.Search)
                  )
              )
              .From(input.From)
              .Size(input.PerPage)
              .Sort(BuildSortExpression(input.OrderBy, input.Order))
      , ct: cancellationToken);

        var videos = response.Documents
            .Select(doc => doc.ToEntity())
            .ToList();

        return new SearchOutput<Video>(input.Page, input.PerPage, (int)response.Total, videos);
    }

    private static Func<SortDescriptor<VideoModel>, IPromise<IList<ISort>>> BuildSortExpression(string orderBy, SearchOrder order)
    => (orderBy.ToLower(), order)
        switch
    {
        ("title", SearchOrder.ASC) => sort => sort
             .Ascending(f => f.Title.Suffix("keyword"))
             .Ascending(f => f.Id),
        ("title", SearchOrder.DESC) => sort => sort
             .Descending(f => f.Title.Suffix("keyword"))
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
             .Ascending(f => f.Title.Suffix("keyword"))
             .Ascending(f => f.Id)
    };
}
