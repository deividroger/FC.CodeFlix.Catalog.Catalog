using Bogus;
using FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.Domain.Repositories.DTOs;
using FC.CodeFlix.Catalog.Infra.ES.Models;

namespace FC.CodeFlix.Catalog.Tests.Shared;

public class GenreDataGenerator: DataGeneratorBase
{
    private readonly CategoryDataGenerator _categoryDataGenerator = new();

    public IList<GenreModel> GetGenreModelList(int count = 10)
     => Enumerable.Range(0, count)
                     .Select(_ =>
                     {
                         Task.Delay(5).GetAwaiter().GetResult();
                         return GenreModel.FromEntity(GetValidGenre());
                     })
                     .ToList();

    public IList<GenreModel> GetGenreModelList(List<string> genreNames)
    => genreNames.Select(name =>
    {
        Task.Delay(5).GetAwaiter().GetResult();
        var genre = GenreModel.FromEntity(GetValidGenre());
        genre.Name = name;
        return genre;
    }).ToList();

    public IList<GenreModel> CloneGenreListOrdered(
    IList<GenreModel> genresList,
    string orderBy,
    SearchOrder direction)
    {
        var listClone = new List<GenreModel>(genresList);
        var orderedEnumerable = (orderBy.ToLower(), direction) switch
        {
            ("name", SearchOrder.ASC) => listClone.OrderBy(x => x.Name)
                .ThenBy(x => x.Id),
            ("name", SearchOrder.DESC) => listClone.OrderByDescending(x => x.Name)
                .ThenByDescending(x => x.Id),
            ("id", SearchOrder.ASC) => listClone.OrderBy(x => x.Id),
            ("id", SearchOrder.DESC) => listClone.OrderByDescending(x => x.Id),
            ("createdat", SearchOrder.ASC) => listClone.OrderBy(x => x.CreatedAt),
            ("createdat", SearchOrder.DESC) => listClone.OrderByDescending(x => x.CreatedAt),
            _ => listClone.OrderBy(x => x.Name).ThenBy(x => x.Id),
        };
        return orderedEnumerable.ToList();
    }

    public List<Genre> GetGenreList(int length = 10)
        => Enumerable
            .Range(0, length)
            .Select(_ => GetValidGenre())
            .ToList();

    public string GetValidName()
        => Faker.Commerce.Categories(1)[0];

    public Genre GetValidGenre(Guid? id = null)
    {
        var categories = new[]
        {
            _categoryDataGenerator.GetValidCategory(),
            _categoryDataGenerator.GetValidCategory(),
        };

        var genre = new Genre(id ?? Guid.NewGuid(),
            GetValidName(),
            GetRandomBoolean(),
            DateTime.Now,
            categories);

        return genre;
    }
}
