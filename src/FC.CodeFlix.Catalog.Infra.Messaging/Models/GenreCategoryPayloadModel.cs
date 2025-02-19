using System.Text.Json.Serialization;

namespace FC.CodeFlix.Catalog.Infra.Messaging.Models;

public class GenreCategoryPayloadModel: GenrePayloadModel
{
    [JsonPropertyName("GenreId")]
    public override Guid Id { get; set; }
}