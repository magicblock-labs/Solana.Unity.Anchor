using Solana.Unity.Anchor.Models.Types;
using Solana.Unity.Anchor.Models.Types.Base;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Solana.Unity.Anchor.Converters
{
    public class IIdlTypeConverter : JsonConverter<IIdlType>
    {
        public override IIdlType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string typeName = reader.GetString();
                //reader.Read();

                return typeName switch
                {
                    "string" => new IdlString(),
                    "publicKey" => new IdlPublicKey(),
                    "pubkey" => new IdlPublicKey(),
                    "bytes" => new IdlArray() { ValuesType = new IdlValueType() { TypeName = "u8" } },
                    "u128" or "i128" => new IdlBigInt() { TypeName = typeName },
                    "UnixTimestamp" => new UnixTimestamp(),
                    _ => new IdlValueType() { TypeName = typeName }
                };
            }
            else if (reader.TokenType == JsonTokenType.StartObject)
            {
                reader.Read();
                if (reader.TokenType != JsonTokenType.PropertyName) throw new JsonException("Unexpected error value.");

                string typeName = reader.GetString();

                if ("defined" == typeName)
                {
                    reader.Read();
                    var readDefinedObj = false;
                    if (reader.TokenType == JsonTokenType.StartObject)
                    {
                        reader.Read();
                        if (reader.TokenType != JsonTokenType.PropertyName) throw new JsonException("Unexpected error value.");
                        string propertyName = reader.GetString();
                        if ("name" != propertyName) throw new JsonException("Unexpected error value.");
                        reader.Read();
                        readDefinedObj = true;
                    }
                    if (reader.TokenType != JsonTokenType.String) throw new JsonException("Unexpected error value.");
                    string definedTypeName = reader.GetString();
                    
                    reader.Read();
                    if (reader.TokenType == JsonTokenType.EndObject && readDefinedObj) reader.Read();
                    return new IdlDefined() { TypeName = definedTypeName };
                }
                else if ("option" == typeName)
                {
                    reader.Read();
                    IIdlType innerType = Read(ref reader, typeToConvert, options);
                    reader.Read();
                    return new IdlOptional() { ValuesType = innerType };
                }
                else if("vec" == typeName)
                {
                    reader.Read();
                    IIdlType innerType = Read(ref reader, typeToConvert, options);
                    reader.Read();
                    return new IdlArray() { ValuesType = innerType };
                }
                else if ("tuple" == typeName || "bTreeMap" == typeName)
                {
                    reader.Read();
                    IIdlTypeArrayConverter idlTypeArrayConverter = new();
                    IIdlType[] innerType = idlTypeArrayConverter.Read(ref reader, typeToConvert, options);
                    reader.Read();
                    return new IdlTuple() { ValuesType = innerType };
                }
                else
                {
                    reader.Read();
                    if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException("Unexpected error value.");
                    reader.Read();
                    IIdlType innerType = Read(ref reader, typeToConvert, options);
                    IIdlType idlType;

                    if ("array" == typeName)
                    {
                        reader.Read();
                        int size = reader.GetInt32();
                        idlType = new IdlArray() { Size = size, ValuesType = innerType };
                    }
                    else
                    {
                        throw new JsonException("unexpected error value");
                    }

                    reader.Read();
                    reader.Read();
                    return idlType;
                }
            }
            throw new JsonException("Unexpected error value.");
        }

        public override void Write(Utf8JsonWriter writer, IIdlType value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}