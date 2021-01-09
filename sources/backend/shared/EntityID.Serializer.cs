using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Elesse.Shared
{

   public class EntityIDMongoSerializer : SerializerBase<EntityID>
   {
      public override EntityID Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args) =>
         EntityID.Parse(context.Reader.ReadString());
      public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, EntityID value) =>
         context.Writer.WriteString(value.ToString());
   }

   public class EntityIDJsonConverter : JsonConverter<EntityID>
   {
      public override EntityID Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
         (EntityID)reader.GetString();
      public override void Write(Utf8JsonWriter writer, EntityID value, JsonSerializerOptions options) =>
         writer.WriteStringValue((string)value);
   }

}
