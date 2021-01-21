using System;
using System.Collections.Generic;
using Elesse.Shared;
using MongoDB.Bson.Serialization.Attributes;

namespace Elesse.Patterns
{

   internal class PatternEntity : ValueObject, IPatternEntity
   {

      public PatternEntity(enPatternType type, Shared.EntityID categoryID, string text)
         : this(Shared.EntityID.NewID(), type, categoryID, text)
      {
         RowsCount = 1;
         RowsDate = DateTime.UtcNow;
      }

      public PatternEntity(Shared.EntityID patternID, enPatternType type, Shared.EntityID categoryID, string text)
      {
         PatternID = patternID;
         Type = type;
         CategoryID = categoryID;
         Text = text;
      }

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

      internal short RowsCount { get; set; }
      internal DateTime RowsDate { get; set; }

      protected override IEnumerable<object> GetAtomicValues()
      {
         yield return PatternID;
         yield return Type;
         yield return CategoryID;
         yield return Text;
      }

   }

   partial struct WARNINGS
   {
      internal const string INVALID_PATTERNID = "INVALID_PATTERNID_PARAMETER";
      internal const string INVALID_CATEGORYID = "INVALID_CATEGORYID_PARAMETER";
      internal const string INVALID_TEXT = "INVALID_TEXT_PARAMETER";
   }

}