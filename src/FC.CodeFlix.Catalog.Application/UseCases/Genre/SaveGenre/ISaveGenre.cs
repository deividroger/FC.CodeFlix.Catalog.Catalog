using FC.CodeFlix.Catalog.Application.UseCases.Genre.Common;
using MediatR;

namespace FC.CodeFlix.Catalog.Application.UseCases.Genre.SaveGenre;

public interface ISaveGenre: IRequestHandler<SaveGenreInput,GenreModelOutput> { 

}
