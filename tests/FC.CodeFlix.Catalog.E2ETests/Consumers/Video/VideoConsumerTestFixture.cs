﻿using FC.CodeFlix.Catalog.E2ETests.Base.Fixture;
using FC.CodeFlix.Catalog.Infra.HttpClients.Models;
using FC.CodeFlix.Catalog.Infra.Messaging.Configuration;
using FC.CodeFlix.Catalog.Infra.Messaging.Models;
using FC.CodeFlix.Catalog.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FC.CodeFlix.Catalog.E2ETests.Consumers.Video;

public class VideoConsumerTestFixture : VideoTestFixtureBase
{
    private readonly KafkaConfiguration _kafkaConfiguration;
    private readonly CategoryDataGenerator _categoryDataGenerator = new();
    private readonly GenreDataGenerator _genreDataGenerator = new();
    private readonly CastMemberDataGenerator _castMemberDataGenerator = new();

    public VideoConsumerTestFixture()
    {
        _kafkaConfiguration = WebAppFactory.Services.GetRequiredService<IOptions<KafkaConfiguration>>().Value;
        // Wait a little for the consumer to be assigned a Partition, mainly in case of rebalacing
        Thread.Sleep(15_000);
    }

    public Task PublishMessageAsync(object message)
        => PublishMessageAsync(
            _kafkaConfiguration.VideoConsumer,
            message);

    public MessageModel<VideoPayloadModel> BuildValidMessage(string operation, Guid id)
    {
        var message = new MessageModel<VideoPayloadModel>
        {
            Payload = new MessageModelPayload<VideoPayloadModel>
            {
                Op = operation
            }
        };
        var videoPayload = new VideoPayloadModel
        {
            Id = id
        };
        if (operation == "d")
        {
            message.Payload.Before = videoPayload;
        }
        else
        {
            message.Payload.After = videoPayload;
        }

        return message;
    }

    public MessageModel<VideoPayloadModel> BuildValidMessage(string operation)
        => BuildValidMessage(operation, Guid.NewGuid());

    public DataWrapper<VideoOutputModel> GetValidAdminCatalogApiResponse(Domain.Entity.Video video)
        => new(new VideoOutputModel
        {
            Id = video.Id,
            CreatedAt = video.CreatedAt,
            Title = video.Title,
            Description = video.Description,
            Rating = video.Rating.ToString("G"),
            YearLaunched = video.YearLaunched,
            Duration = video.Duration,
            Categories = video.Categories.Select(x => new VideoModelOutputRelation
            {
                Id = x.Id,
                Name = x.Name
            }).ToList(),
            Genres = video.Genres.Select(x => new VideoModelOutputRelation
            {
                Id = x.Id,
                Name = x.Name
            }).ToList(),
            CastMembers = video.CastMembers.Select(x => new VideoModelOutputRelation
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList(),
            ThumbFileUrl = video.Medias.ThumbUrl,
            BannerFileUrl = video.Medias.BannerUrl,
            ThumbHalfFileUrl = video.Medias.ThumbHalfUrl,
            VideoFileUrl = video.Medias.MediaUrl,
            TrailerFileUrl = video.Medias.TrailerUrl
        });

    public Domain.Entity.Video GetValidVideo(Guid id)
    {
        var video = DataGenerator.GetValidVideo(id);
        video.AddCategories(_categoryDataGenerator.GetValidCategory());
        video.AddGenres(_genreDataGenerator.GetValidGenre());
        video.AddCastMembers(_castMemberDataGenerator.GetValidCastMember());
        return video;
    }
}

[CollectionDefinition(nameof(VideoConsumerTestFixture))]
public class VideoConsumerTestFixtureCollection
    : ICollectionFixture<VideoConsumerTestFixture>
{
}