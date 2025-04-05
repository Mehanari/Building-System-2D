using System;
using Newtonsoft.Json;
using UnityEngine;

namespace BuildingSystem2D.Core.JsonConverters
{
    public class Vector2Converter : JsonConverter<Vector2>
    {

        public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            Vector2 result = default(Vector2);

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    switch (reader.Value.ToString())
                    {
                        case "x":
                            result.x = (float)reader.ReadAsDouble().Value;
                            break;
                        case "y":
                            result.y = (float)reader.ReadAsDouble().Value;
                            break;
                    }
                }
                else if (reader.TokenType == JsonToken.EndObject)
                    break;
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
        {
            writer.WriteStartObject();            
            if(serializer.TypeNameHandling != TypeNameHandling.None)
            {
                writer.WritePropertyName("$type");
                writer.WriteValue($"{value.GetType().ToString()}, {value.GetType().Assembly.GetName().Name}");
            }                
            writer.WritePropertyName("x");
            writer.WriteValue(value.x);
            writer.WritePropertyName("y");
            writer.WriteValue(value.y);
            writer.WriteEndObject();
        }
    }
}