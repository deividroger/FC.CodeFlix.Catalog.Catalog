﻿using FC.CodeFlix.Catalog.Application.UseCases.Genre.Common;
using FC.CodeFlix.Catalog.Domain.Repositories;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;

namespace FC.CodeFlix.Catalog.Application.UseCases.Genre.SaveGenre;

public class SaveGenre : ISaveGenre
{
    private readonly IGenreRepository _repository;

    public SaveGenre(IGenreRepository repository)
        => _repository = repository;

    public async Task<GenreModelOutput> Handle(SaveGenreInput request, CancellationToken cancellationToken)
    {
        var genre = new DomainEntity.Genre(request.Id,
                request.Name,
                request.IsActive,
                request.CreatedAt,
                request.Categories
                .Select(item => new DomainEntity.Category(item.Id, item.Name)));

        await _repository.SaveAsync(genre, cancellationToken);

        return GenreModelOutput.FromGenre(genre);
    }
}
