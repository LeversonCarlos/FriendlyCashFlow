using System;
using Elesse.Shared;

namespace Elesse.Patterns
{

   partial class PatternEntity
   {

      public static PatternEntity Create(enPatternType type, EntityID categoryID, string text) =>
         new PatternEntity
         {
            PatternID = EntityID.NewID(),
            Type = type,
            CategoryID = categoryID,
            Text = text,
            RowsDate = DateTime.UtcNow,
            RowsCount = 0
         };

   }

}
