using System;
using System.Collections.Generic;
using Elesse.Shared;
using MongoDB.Bson.Serialization.Attributes;

namespace Elesse.Patterns
{

   internal partial class PatternEntity : ValueObject, IPatternEntity
   {

      private PatternEntity() { }

      Shared.EntityID _PatternID;
      [BsonId]
      public Shared.EntityID PatternID
      {
         get => _PatternID;
         private set
         {
            if (value == null)
               throw new ArgumentException(WARNINGS.INVALID_PATTERNID);
            _PatternID = value;
         }
      }

      enPatternType _Type;
      public enPatternType Type
      {
         get => _Type;
         private set
         {
            _Type = value;
         }
      }

      Shared.EntityID _CategoryID;
      public Shared.EntityID CategoryID
      {
         get => _CategoryID;
         private set
         {
            if (value == null)
               throw new ArgumentException(WARNINGS.INVALID_CATEGORYID);
            _CategoryID = value;
         }
      }

      string _Text;
      public string Text
      {
         get => _Text;
         private set
         {
            if (string.IsNullOrWhiteSpace(value) || value.Length > 100)
               throw new ArgumentException(WARNINGS.INVALID_TEXT);
            _Text = value;
         }
      }

      public short RowsCount { get; set; }
      public DateTime RowsDate { get; set; }

      protected override IEnumerable<object> GetAtomicValues()
      {
         yield return PatternID;
         yield return Type;
         yield return CategoryID;
         yield return Text;
      }

      public static PatternEntity Create(enPatternType type, EntityID categoryID, string text) =>
         new PatternEntity
         {
            PatternID = EntityID.NewID(),
            Type = type,
            CategoryID = categoryID,
            Text = text,
            RowsDate = DateTime.UtcNow,
            RowsCount = 1
         };

      // [BsonFactoryMethod]
      public static PatternEntity Restore(EntityID patternID, enPatternType type, EntityID categoryID, string text) =>
         new PatternEntity
         {
            PatternID = patternID,
            Type = type,
            CategoryID = categoryID,
            Text = text
         };

   }

   partial struct WARNINGS
   {
      internal const string INVALID_PATTERNID = "INVALID_PATTERNID";
      internal const string INVALID_CATEGORYID = "INVALID_CATEGORYID";
      internal const string INVALID_TEXT = "INVALID_TEXT";
   }

}
