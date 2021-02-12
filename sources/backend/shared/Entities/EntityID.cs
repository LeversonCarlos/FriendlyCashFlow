using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Elesse.Shared
{

   [JsonConverter(typeof(EntityIDJsonConverter))]
   [BsonSerializer(typeof(EntityIDMongoSerializer)) /*, BsonRepresentation(BsonType.String)*/]
   public partial class EntityID : EntityID<Guid>
   {

      public EntityID(Guid guid) : base(guid) { }

      public static EntityID NewID() => EntityID.Parse(Guid.NewGuid());
      public static EntityID Parse(Guid guid) => new EntityID(guid);
      public static EntityID Parse(string guidString)
      {
         if (string.IsNullOrWhiteSpace(guidString))
            throw new ArgumentException("The string argument to parse an EntityID type cannot be empty");
         if (!Guid.TryParse(guidString, out Guid guid))
            throw new ArgumentException($"The string argument [{guidString}] received to parse an EntityID type is invalid");
         return EntityID.Parse(guid);
      }

      public static bool TryParse(string inputID, out EntityID outputID)
      {
         try
         {
            outputID = Parse(inputID);
            return true;
         }
         catch { outputID = null; return false; }
      }

      public long ToLong()
      {

         var bytes = Value.ToByteArray();
         Array.Resize(ref bytes, 17);

         var bigInt = new System.Numerics.BigInteger(bytes);

         var logValue = System.Numerics.BigInteger.Log10(bigInt);
         var doubleValue = logValue * Math.Pow(10, 15);
         var intValue = Math.Round(doubleValue, 0);

         return (long)intValue;
      }

      // implicit
      public static explicit operator string(EntityID id) => id.Value.ToString();
      public static explicit operator EntityID(string id) => EntityID.Parse(id);
   }

   public class EntityID<T> : ValueObject
   {

      public T Value { get; }

      protected EntityID(T value) => Value = value;

      protected override IEnumerable<object> GetAtomicValues()
      {
         yield return Value;
      }

      // public static implicit operator T(EntityID<T> id) => id.Value;
      // public static explicit operator EntityID<T>(T value) => new EntityID<T>(value);
      // public static explicit operator T(EntityID<T> id) => id.Value;

      public override string ToString() => this.Value?.ToString() ?? string.Empty;

   }

}
