using System;

namespace Elesse.Entries
{

   partial class EntryEntity
   {

      public static EntryEntity Create(Patterns.IPatternEntity pattern, Shared.EntityID accountID,
         DateTime dueDate, decimal entryValue) =>
         new EntryEntity
         {
            EntryID = Shared.EntityID.NewID(),
            Pattern = pattern,
            AccountID = accountID,
            DueDate = dueDate,
            EntryValue = entryValue
         };

   }

}
