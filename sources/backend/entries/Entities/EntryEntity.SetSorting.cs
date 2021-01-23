using System;

namespace Elesse.Entries
{

   partial class EntryEntity
   {

      public void RefreshSorting()
      {
         SearchDate = DueDate;
         if (Paid && PayDate.HasValue)
            SearchDate = PayDate.Value;

         Sorting = System.Convert.ToInt64(SearchDate.Subtract(new DateTime(1901, 1, 1)).TotalDays);

         var longEntityID = EntryID.ToLong();
         Sorting += ((decimal)longEntityID / (decimal)Math.Pow(10, longEntityID.ToString().Length));
      }

   }

}
