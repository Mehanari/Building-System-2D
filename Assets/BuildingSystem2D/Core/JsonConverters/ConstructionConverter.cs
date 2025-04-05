using System;
using Newtonsoft.Json;

namespace BuildingSystem2D.Core.JsonConverters
{
    public class ConstructionConverter : JsonConverter<Construction>
    {
        public override void WriteJson(JsonWriter writer, Construction value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override Construction ReadJson(JsonReader reader, Type objectType, Construction existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}