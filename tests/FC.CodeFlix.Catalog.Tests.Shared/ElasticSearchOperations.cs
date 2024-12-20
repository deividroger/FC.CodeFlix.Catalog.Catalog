﻿using Elasticsearch.Net;
using FC.CodeFlix.Catalog.Infra.ES;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using Nest;

namespace FC.CodeFlix.Catalog.Tests.Shared;

public static class ElasticSearchOperations
{
    public static async Task CreateCategoryIndexAsync(IElasticClient elasticClient)
    {
        var existsResponse = await elasticClient.Indices.ExistsAsync(ElasticsearchIndices.Category);

        if (existsResponse.Exists)
        {
            await elasticClient.Indices.DeleteAsync(ElasticsearchIndices.Category);
        }


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

    public static void DeleteCategoryDocuments(IElasticClient elasticClient)
    {
        elasticClient.DeleteByQuery<CategoryModel>(del =>
            del.Query(
                q => q.QueryString(qs => qs.Query("*"))
            ).Conflicts(Conflicts.Proceed)
        );
    }

    public static void DeleteCategoryIndex(IElasticClient elasticClient)
    {
        elasticClient.Indices.Delete(ElasticsearchIndices.Category);
    }
}
