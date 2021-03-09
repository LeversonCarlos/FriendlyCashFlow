using System;

namespace Elesse.Recurrences
{

   partial class RecurrenceEntity
   {

      public static RecurrenceEntity Restore(
         Shared.EntityID recurrenceID, Shared.EntityID patternID,
         Shared.EntityID accountID, DateTime date, decimal value,
         enRecurrenceType type) =>
         new RecurrenceEntity
         {
            RecurrenceID = recurrenceID,
            PatternID = patternID,
            AccountID = accountID,
            Date = date,
            Value = value,
            Type = type
         };

   }

}
