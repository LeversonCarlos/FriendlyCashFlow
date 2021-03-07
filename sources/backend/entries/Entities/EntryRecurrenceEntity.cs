using System;
using Elesse.Shared;
using System.Collections.Generic;

namespace Elesse.Entries
{

   internal class EntryRecurrenceEntity : ValueObject, IEntryRecurrenceEntity
   {

      Shared.EntityID _RecurrencyID;
      public Shared.EntityID RecurrencyID
      {
         get => _RecurrencyID;
         private set
         {
            if (value == null)
               throw new ArgumentException(WARNINGS.INVALID_RECURRENCYID);
            _RecurrencyID = value;
         }
      }

      short _CurrentOccurrence;
      public short CurrentOccurrence
      {
         get => _CurrentOccurrence;
         private set
         {
            if (value <= 0 || value > short.MaxValue)
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
            if (value <= 0 || value > short.MaxValue)
               throw new ArgumentException(WARNINGS.INVALID_TOTALOCCURRENCES);
            _TotalOccurrences = value;
         }
      }

      protected override IEnumerable<object> GetAtomicValues()
      {
         yield return RecurrencyID;
         yield return CurrentOccurrence;
         yield return TotalOccurrences;
      }

   }

}
