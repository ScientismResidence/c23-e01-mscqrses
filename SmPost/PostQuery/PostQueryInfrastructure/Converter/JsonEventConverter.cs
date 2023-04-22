using System.Text.Json;
using System.Text.Json.Serialization;
using CqrsCore.Event;
using PostCommon.Event;

namespace PostQueryInfrastructure.Converter;

public class JsonEventConverter : JsonConverter<EventBase>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsAssignableFrom(typeof(EventBase));
    }

    public override EventBase Read(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!JsonDocument.TryParseValue(ref reader, out JsonDocument document))
        {
            throw new JsonException("Failed to parse JsonDocument");
        }
        
        if (!document.RootElement.TryGetProperty(nameof(EventBase.Type), out JsonElement typeElement))
        {
            throw new JsonException("Unable to find the type discriminator property");
        }
        
        string typeDiscriminator = typeElement.GetString();
        string json = document.RootElement.GetRawText();
        
        return typeDiscriminator switch
        {
            nameof(CommentAddedEvent) => JsonSerializer.Deserialize<CommentAddedEvent>(json, options),
            nameof(CommentRemovedEvent) => JsonSerializer.Deserialize<CommentRemovedEvent>(json, options),
            nameof(CommentUpdatedEvent) => JsonSerializer.Deserialize<CommentUpdatedEvent>(json, options),
            nameof(PostAddedEvent) => JsonSerializer.Deserialize<PostAddedEvent>(json, options),
            nameof(PostLikedEvent) => JsonSerializer.Deserialize<PostLikedEvent>(json, options),
            nameof(PostRemovedEvent) => JsonSerializer.Deserialize<PostRemovedEvent>(json, options),
            nameof(PostUpdatedEvent) => JsonSerializer.Deserialize<PostUpdatedEvent>(json, options),
            _ => throw new JsonException($"Unable to deserialize type {typeDiscriminator}")
        };
    }

    public override void Write(
        Utf8JsonWriter writer, EventBase value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}