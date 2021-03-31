using System;

namespace Elesse.Entries
{

   partial class EntryEntity
   {

      public static EntryEntity Restore(Shared.EntityID entryID, Patterns.IPatternEntity pattern, Shared.EntityID accountID,
         DateTime dueDate, decimal value, bool paid, DateTime? payDate, IEntryRecurrenceEntity recurrence = null) =>
         new EntryEntity
         {
            EntryID = entryID,
            Pattern = pattern,
            AccountID = accountID,
            DueDate = dueDate,
            Value = value,
            Paid = paid,
            PayDate = payDate,
            Recurrence = recurrence
         };

      public static EntryEntity Create(Patterns.IPatternEntity pattern, Shared.EntityID accountID,
         DateTime dueDate, decimal value) =>
         new EntryEntity
         {
            EntryID = Shared.EntityID.NewID(),
            Pattern = pattern,
            AccountID = accountID,
            DueDate = dueDate,
            Value = value
         };

   }

}
