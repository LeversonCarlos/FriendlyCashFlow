using System;
using Elesse.Shared;
using System.Collections.Generic;

namespace Elesse.Entries
{

   internal partial class EntryRecurrenceEntity : ValueObject, IEntryRecurrenceEntity
   {
      public const short MaxOccurrence = 1000;

      Shared.EntityID _RecurrenceID;
      public Shared.EntityID RecurrenceID
      {
         get => _RecurrenceID;
         private set
         {
            if (value == null)
               throw new ArgumentException(WARNINGS.INVALID_RECURRENCEID);
            _RecurrenceID = value;
         }
      }

      short _CurrentOccurrence;
      public short CurrentOccurrence
      {
         get => _CurrentOccurrence;
         private set
         {
            if (value <= 0 || value > MaxOccurrence)
               throw new ArgumentException(WARNINGS.INVALID_CURRENTOCCURRENCE);
            _CurrentOccurrence = value;
         }
      }

      short _TotalOccurrences;
      public short TotalOccurrences
      {
         get => _TotalOccurrences;
         private set
         {
            if (value <= 0 || value > MaxOccurrence)
               throw new ArgumentException(WARNINGS.INVALID_TOTALOCCURRENCES);
            _TotalOccurrences = value;
         }
      }

      protected override IEnumerable<object> GetAtomicValues()
      {
         yield return RecurrenceID;
         yield return CurrentOccurrence;
         yield return TotalOccurrences;
      }

   }

}
