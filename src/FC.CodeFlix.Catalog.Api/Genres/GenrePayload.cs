using FC.CodeFlix.Catalog.Application.UseCases.Genre.Common;

namespace FC.CodeFlix.Catalog.Api.Genres;

public class GenrePayload
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public IEnumerable<GenreCategoryPayload>? Categories { get; set; }

    public static GenrePayload FromGenreModelOutput(GenreModelOutput output)
    => new()
    {
        Id = output.Id,
        Name = output.Name,
        CreatedAt = output.CreatedAt,
        IsActive = output.IsActive,
        Categories = output.Categories.Select(y => new GenreCategoryPayload(y.Id, y.Name)).ToList()

    };

}


public class GenreCategoryPayload
{
    public GenreCategoryPayload(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; set; }

    public string? Name { get; set; }
}