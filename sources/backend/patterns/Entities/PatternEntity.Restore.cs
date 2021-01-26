using Elesse.Shared;

namespace Elesse.Patterns
{

   partial class PatternEntity
   {

      // [BsonFactoryMethod]
      public static PatternEntity Restore(EntityID patternID, enPatternType type, EntityID categoryID, string text)
      {
         if (patternID != null)
            return new PatternEntity
            {
               PatternID = patternID,
               Type = type,
               CategoryID = categoryID,
               Text = text
            };
         else
            return Create(type, categoryID, text);
      }

   }

}
