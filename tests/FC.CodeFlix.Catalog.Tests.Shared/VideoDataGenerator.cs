using FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.Domain.ValueObjects;
using FC.CodeFlix.Catalog.Domain.Enums;
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

    public List<Video> GetVideoList(int length = 10)
        => Enumerable
            .Range(0, length)
            .Select(_ => GetValidVideo())
            .ToList();
}
