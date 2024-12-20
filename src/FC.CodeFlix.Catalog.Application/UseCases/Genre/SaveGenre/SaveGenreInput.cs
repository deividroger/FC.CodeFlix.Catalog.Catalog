using FC.CodeFlix.Catalog.Application.UseCases.Genre.Common;
using MediatR;

namespace FC.CodeFlix.Catalog.Application.UseCases.Genre.SaveGenre;

public class SaveGenreInput: IRequest<GenreModelOutput>
{
    public SaveGenreInput(Guid id, string name, bool isActive, DateTime createdAt, IEnumerable<SaveGenreInputCategory> categories)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        CreatedAt = createdAt;
        Categories = categories;
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public IEnumerable<SaveGenreInputCategory> Categories { get; private set; }
}



public class SaveGenreInputCategory
{
    public Guid Id { get; set; }
    public string? Name { get; set; }

    public SaveGenreInputCategory(Guid id, string? name = null)
        => (Id, Name) = (id, name);

}