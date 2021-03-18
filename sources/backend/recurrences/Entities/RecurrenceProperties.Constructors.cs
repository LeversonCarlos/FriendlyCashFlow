using System;

namespace Elesse.Recurrences
{
   partial class RecurrenceProperties
   {

      public static RecurrenceProperties Create(
         Shared.EntityID patternID,
         Shared.EntityID accountID, DateTime date, decimal value,
         enRecurrenceType type) =>
         new RecurrenceProperties
         {
            PatternID = patternID,
            AccountID = accountID,
            Date = date,
            Value = value,
            Type = type
         };

   }
}
