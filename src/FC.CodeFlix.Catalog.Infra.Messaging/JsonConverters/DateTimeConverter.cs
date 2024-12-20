﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace FC.CodeFlix.Catalog.Infra.Messaging.JsonConverters;

internal class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => DateTime.UnixEpoch.AddMicroseconds(reader.GetInt64());

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        => writer.WriteNumberValue(((DateTimeOffset)value).ToUnixTimeMilliseconds() * 1_000);
}
