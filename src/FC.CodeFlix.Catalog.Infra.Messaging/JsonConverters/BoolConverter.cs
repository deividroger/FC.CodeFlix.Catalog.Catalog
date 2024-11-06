using System.Text.Json;
using System.Text.Json.Serialization;

namespace FC.CodeFlix.Catalog.Infra.Messaging.JsonConverters;

public class BoolConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.TokenType switch
        {
            JsonTokenType.True => true,
            JsonTokenType.False => false,
            JsonTokenType.String => bool.TryParse(reader.GetString(), out var b) ? b : throw new JsonException(),   
            JsonTokenType.Number => reader.TryGetInt64(out long value)
                ? Convert.ToBoolean(value) 
                : reader.TryGetDouble(out double d) && Convert.ToBoolean(d),
            _ => throw new JsonException()
        };

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        => writer.WriteNumberValue(value ? 1 : 0);
}
