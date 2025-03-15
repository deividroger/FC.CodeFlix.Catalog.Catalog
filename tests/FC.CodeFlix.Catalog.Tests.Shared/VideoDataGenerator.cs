using FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.Domain.ValueObjects;
using FC.CodeFlix.Catalog.Domain.Enums;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using FC.CodeFlix.Catalog.Domain.Repositories.DTOs;
namespace FC.CodeFlix.Catalog.Tests.Shared;

public class VideoDataGenerator: DataGeneratorBase
{
    public Video GetValidVideo(Guid? id = null)
        => new (id ?? Guid.NewGuid(), 
                GetValidTitle(), 
                GetValidDescription(), 
                GetValidYearLaunched(),
                GetValidDuration(),
                DateTime.Now.Date,
                GetRandomRating(), 
                GetValidMedias());

    public string GetValidTitle()
        => Faker.Lorem.Letter(100);

    public string GetValidDescription()
        => Faker.Commerce.ProductDescription();

    public int GetValidYearLaunched()
        => Faker.Date.BetweenDateOnly(new DateOnly(1900, 1, 1), new DateOnly(2100, 1, 1)).Year; 

    public int GetValidDuration()
        => Faker.Random.Int(100, 300);

    public string GetValidUrl()
        => Faker.Internet.UrlWithPath();

    public Rating GetRandomRating()
    {
        var enumValue = Enum.GetValues<Rating>();
        var random = new Random();

        return enumValue[random.Next(enumValue.Length)];
    }

    public Medias GetValidMedias()
        => new(GetValidUrl(), GetValidUrl(), GetValidUrl(), GetValidUrl(), GetValidUrl());


    public List<VideoModel> GetVideoModelList(int count = 10)
     => Enumerable.Range(0, count)
                     .Select(_ =>
                     {
                         Task.Delay(5).GetAwaiter().GetResult();
                         return VideoModel.FromEntity(GetValidVideo());
                     })
                     .ToList();

    public List<VideoModel> GetVideoModelList(IEnumerable<string> titles)
       => titles
           .Select(title =>
           {
               var video = GetValidVideo();
               var model = VideoModel.FromEntity(video);
               model.Title = title;
               return model;
           })
           .ToList();

    public IList<VideoModel> CloneVideosListOrdered(
       List<VideoModel> examples, string orderBy, SearchOrder inputOrder)
    {
        var listClone = new List<VideoModel>(examples);
        var orderedEnumerable = (orderBy.ToLower(), inputOrder) switch
        {
            ("title", SearchOrder.ASC) => listClone.OrderBy(x => x.Title)
                .ThenBy(x => x.Id),
            ("title", SearchOrder.DESC) => listClone.OrderByDescending(x => x.Title)
                .ThenByDescending(x => x.Id),
            ("id", SearchOrder.ASC) => listClone.OrderBy(x => x.Id),
            ("id", SearchOrder.DESC) => listClone.OrderByDescending(x => x.Id),
            ("createdat", SearchOrder.ASC) => listClone.OrderBy(x => x.CreatedAt),
            ("createdat", SearchOrder.DESC) => listClone.OrderByDescending(x => x.CreatedAt),
            _ => listClone.OrderBy(x => x.Title).ThenBy(x => x.Id),
        };
        return orderedEnumerable.ToList();
    }

    public List<Video> GetVideoList(int length = 10)
        => Enumerable
            .Range(0, length)
            .Select(_ => GetValidVideo())
            .ToList();
}
