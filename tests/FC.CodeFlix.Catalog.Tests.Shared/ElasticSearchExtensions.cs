﻿using Elasticsearch.Net;
using FC.CodeFlix.Catalog.Infra.ES;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using Nest;

namespace FC.CodeFlix.Catalog.Tests.Shared;

public static class ElasticSearchExtensions
{
    public static async Task CreateGenreIndexAsync(this IElasticClient elasticClient)
    {
        await elasticClient.DeleteIndexAsync(ElasticsearchIndices.Genre);

        _ = await elasticClient.Indices.CreateAsync(ElasticsearchIndices.Genre, c => c
           .Map<GenreModel>(m => m
               .Properties(ps => ps
                   .Keyword(t => t
                       .Name(genre => genre.Id)
                   )
                .Date(t => t.Name(genre => genre.CreatedAt))
                .Boolean(t => t.Name(genre => genre.IsActive))
                .Text(t => t.Name(genre => genre.Name)
                    .Fields(fs => fs.Keyword(t => t.Name(genre => genre.Name.Suffix("keyword"))))
                )
                .Nested<GenreCategoryModel>(n => n
                    .Name(genre=> genre.Categories)
                    .Properties(pss => pss
                        .Keyword(t => t.Name(x => x.Id))
                        .Keyword(t => t.Name(x => x.Name))
                    ))
            )
           )
       );
    }

    private static async Task DeleteIndexAsync(this IElasticClient elasticClient, string indexName)
    {
        var existsResponse = await elasticClient.Indices.ExistsAsync(indexName);

        if (existsResponse.Exists)
        {
            await elasticClient.Indices.DeleteAsync(indexName);
        }
    }

    public static async Task CreateCategoryIndexAsync(this IElasticClient elasticClient)
    {
        await elasticClient.DeleteIndexAsync(ElasticsearchIndices.Category);

        _ = await elasticClient.Indices.CreateAsync(ElasticsearchIndices.Category, c => c
            .Map<CategoryModel>(m => m
                .Properties(ps => ps
                    .Keyword(t => t
                        .Name(category => category.Id)
                    )
                    .Text(t => t
                        .Name(category => category.Name)
                        .Fields(fs => fs
                            .Keyword(k => k
                                .Name(category => category.Name.Suffix("keyword")))
                        )
                    )
                    .Text(t => t
                        .Name(category => category.Description)
                    )
                    .Boolean(b => b
                        .Name(category => category.IsActive)
                    )
                    .Date(d => d
                        .Name(category => category.CreatedAt)
                    )
                )
            )
        );
    }

    public static async Task CreateCastMemberIndexAsync(this IElasticClient elasticClient)
    {
        await elasticClient.DeleteIndexAsync(ElasticsearchIndices.CastMember);
        _ = await elasticClient.Indices.CreateAsync(ElasticsearchIndices.CastMember, c => c
            .Map<CastMemberModel>(m=> m
            .Properties(ps=> ps
                .Keyword (t => t
                .Name(castMember => castMember.Id)
                )
                .Text(t => t
                .Name(castMember => castMember.Name)
                .Fields(fs => fs.Keyword(t => t.Name(castMember => castMember.Name.Suffix("keyword"))))
                ).Number(t => t
                .Name(castMember => castMember.Type)

                ).
                Date(t => t.Name(castMember => castMember.CreatedAt)
            )
            ))
        );
    }

    public static void DeleteDocuments<T>(this IElasticClient elasticClient)
        where T : class
    {
        elasticClient.DeleteByQuery<T>(del =>
            del.Query(
                q => q.QueryString(qs => qs.Query("*"))
            ).Conflicts(Conflicts.Proceed)
        );
    }

    public static void DeleteCategoryIndex(this IElasticClient elasticClient)
       => elasticClient.Indices
        .Delete(ElasticsearchIndices.Category);

    public static void DeleteGenreIndex(this IElasticClient elasticClient)
        => elasticClient.Indices
        .Delete(ElasticsearchIndices.Genre);

    public static void DeleteCastMemberIndex(this IElasticClient elasticClient)
        => elasticClient.Indices
        .Delete(ElasticsearchIndices.CastMember);
}
