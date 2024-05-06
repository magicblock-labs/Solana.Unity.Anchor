using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Solana.Unity.Anchor.Converters
{
    public class IIdlTypeNameConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == "name")
                    {
                        reader.Read();
                        return reader.GetString();
                    }
                }
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                return reader.GetString();
            }

            throw new JsonException("Unable to parse the 'name' property.");
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}