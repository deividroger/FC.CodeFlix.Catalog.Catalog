﻿using FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.Domain.Enums;
using FC.CodeFlix.Catalog.Domain.ValueObjects;

namespace FC.CodeFlix.Catalog.Infra.ES.Models;

public class VideoModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int YearLaunched { get; set; }
    public int Duration { get; set; }
    public DateTime CreatedAt { get; set; }
    public Rating Rating { get; set; }
    public string? ThumbUrl { get; set; }
    public string? ThumbHalfUrl { get; set; }
    public string? BannerUrl { get; set; }
    public string? MediaUrl { get; set; }
    public string? TrailerUrl { get; set; }
    public List<VideoCategoryModel>? Categories { get; set; }
    public List<VideoGenreModel>? Genres { get; set; }
    public List<VideoCastMemberModel>? CastMembers { get; set; }

    public static VideoModel FromEntity(Video video)
        => new()
        {
            Id = video.Id,
            Title = video.Title,
            Description = video.Description,
            YearLaunched = video.YearLaunched,
            Duration = video.Duration,
            CreatedAt = video.CreatedAt,
            Rating = video.Rating,
            ThumbUrl = video.Medias.ThumbUrl,
            ThumbHalfUrl = video.Medias.ThumbHalfUrl,
            BannerUrl = video.Medias.BannerUrl,
            MediaUrl = video.Medias.MediaUrl,
            TrailerUrl = video.Medias.TrailerUrl,
            Categories = video.Categories.Select(c => new VideoCategoryModel(c.Id, c.Name)).ToList(),
            Genres = video.Genres.Select(g => new VideoGenreModel(g.Id, g.Name)).ToList(),
            CastMembers = video.CastMembers.Select(c => new VideoCastMemberModel(c.Id, c.Name, c.Type)).ToList()
        };

    public Video ToEntity()
    {
        var video = new Video(
            Id,
            Title,
            Description,
            YearLaunched,
            Duration,
            CreatedAt,
            Rating,
            new Medias(ThumbUrl, ThumbHalfUrl, BannerUrl, MediaUrl, TrailerUrl));

        video.AddCategories(Categories?.Select(item => new Category(item.Id, item.Name)).ToArray() ??
                            Array.Empty<Category>());
        video.AddGenres(Genres?.Select(item => new Genre(item.Id, item.Name)).ToArray() ?? Array.Empty<Genre>());
        video.AddCastMembers(CastMembers?.Select(item => new CastMember(item.Id, item.Name, item.Type)).ToArray() ??
                             Array.Empty<CastMember>());
        return video;
    }
}

public class VideoGenreModel
{
    public VideoGenreModel(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
}

public class VideoCategoryModel
{
    public VideoCategoryModel(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
}

public class VideoCastMemberModel
{
    public VideoCastMemberModel(Guid id, string name, CastMemberType type)
    {
        Id = id;
        Name = name;
        Type = type;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public CastMemberType Type { get; set; }
}
