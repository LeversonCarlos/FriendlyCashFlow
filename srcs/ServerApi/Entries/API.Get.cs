using System;

namespace FriendlyCashFlow.API.Entries
{

   partial class EntriesService
   {

      private void ApplySorting(EntryData data)
      {
         try
         {
            data.Sorting = Convert.ToInt64(data.SearchDate.Subtract(new DateTime(1901, 1, 1)).TotalDays);
            data.Sorting += ((decimal)data.EntryID / (decimal)Math.Pow(10, data.EntryID.ToString().Length));
         }
         catch (Exception) { throw; }
      }

   }

}
