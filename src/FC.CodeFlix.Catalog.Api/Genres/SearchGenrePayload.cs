using FC.CodeFlix.Catalog.Api.Common;
using FC.CodeFlix.Catalog.Application.Common;
using FC.CodeFlix.Catalog.Application.UseCases.Genre.Common;

namespace FC.CodeFlix.Catalog.Api.Genres;

public class SearchGenrePayload : SearchPayload<GenrePayload>
{
    public static SearchGenrePayload FromSearchListOutput(SearchListOutput<GenreModelOutput> output)
       => new()
       {
           CurrentPage = output.CurrentPage,
           PerPage = output.PerPage,
           Total = output.Total,
           Items = output.Items.Select(GenrePayload.FromGenreModelOutput).ToList()

       };
}
