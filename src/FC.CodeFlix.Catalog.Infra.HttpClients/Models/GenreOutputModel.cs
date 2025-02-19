using FC.CodeFlix.Catalog.Domain.Entity;

namespace FC.CodeFlix.Catalog.Infra.HttpClients.Models;

public class GenreOutputModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public IEnumerable<GenreCategoryOutputModel> Categories { get; set; } = null!;

    public Genre ToGenre()
         => new(Id, 
             Name, 
             IsActive,
             CreatedAt,
             Categories?.Select(x => new Category(x.Id, x.Name)));

}

public class GenreCategoryOutputModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;


}