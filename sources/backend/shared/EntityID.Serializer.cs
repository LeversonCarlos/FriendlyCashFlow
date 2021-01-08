using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Elesse.Shared
{

   public class EntityIDSerializer : SerializerBase<EntityID>
   {
      public override EntityID Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
      {
         string val = context.Reader.ReadString();
         return new EntityID(val);
      }

      public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, EntityID value)
      {
         context.Writer.WriteString(value.ToString());
      }
   }

}
