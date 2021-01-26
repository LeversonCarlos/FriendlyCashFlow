using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Elesse.Shared;

namespace Elesse.Patterns
{

   [JsonInterfaceConverter(typeof(PatternEntityConverter))]
   partial interface IPatternEntity { }

   internal class PatternEntityDTO : IPatternEntity
   {
      public EntityID PatternID { get; set; }
      public enPatternType Type { get; set; }
      public EntityID CategoryID { get; set; }
      public string Text { get; set; }
   }

   internal class PatternEntityConverter : JsonConverter<IPatternEntity>
   {

      public override IPatternEntity Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
      {
         var dto = (PatternEntityDTO)JsonSerializer.Deserialize(ref reader, typeof(PatternEntityDTO), options);
         PatternEntity entity;
         if (dto.PatternID != null)
            entity = PatternEntity.Restore(dto.PatternID, dto.Type, dto.CategoryID, dto.Text);
         else
            entity = PatternEntity.Create(dto.Type, dto.CategoryID, dto.Text);
         return entity;
      }

      public override void Write(Utf8JsonWriter writer, IPatternEntity value, JsonSerializerOptions options)
      {
         JsonSerializer.Serialize(writer, value, typeof(PatternEntity), options);
      }

   }

}
