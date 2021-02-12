using System;

namespace Elesse.Entries
{

   partial class EntryEntity
   {

      public static EntryEntity Restore(Shared.EntityID entryID, Patterns.IPatternEntity pattern, Shared.EntityID accountID,
         DateTime dueDate, decimal entryValue, bool paid, DateTime? payDate) =>
         new EntryEntity
         {
            EntryID = entryID,
            Pattern = pattern,
            AccountID = accountID,
            DueDate = dueDate,
            EntryValue = entryValue,
            Paid = paid,
            PayDate = payDate
         };

   }

}
