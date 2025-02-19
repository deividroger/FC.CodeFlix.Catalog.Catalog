using FC.CodeFlix.Catalog.Application.UseCases.Genre.Common;
using MediatR;

namespace FC.CodeFlix.Catalog.Application.UseCases.Genre.SaveGenre;

public class SaveGenreInput: IRequest<GenreModelOutput>
{
    public SaveGenreInput(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private set; }
}