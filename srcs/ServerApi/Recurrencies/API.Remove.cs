using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Recurrencies
{

   partial class RecurrenciesService
   {

      internal async Task<bool> RemoveAsync(long recurrencyID)
      {
         try
         {

            // LOCATE ROW
            var data = await this.GetDataQuery().Where(x => x.RecurrencyID == recurrencyID).FirstOrDefaultAsync();
            if (data == null) { return false; }

            // CHECK FOR REMINING ENTRIES
            var remainingEntries = await this.dbContext.Entries.Where(x => x.RecurrencyID == recurrencyID).CountAsync();
            if (remainingEntries > 0) { return true; }

            // APPLY
            data.RowStatus = -1;
            this.dbContext.Remove(data);
            await this.dbContext.SaveChangesAsync();

            // RESULT
            return true;
         }
         catch (Exception) { throw; }
      }

      internal async Task<int> RemoveEntriesAsync(long recurrencyID, long lastEntryToKeepID)
      {
         try
         {

            // LOCATE ROW
            var data = await this.GetDataQuery().Where(x => x.RecurrencyID == recurrencyID).FirstOrDefaultAsync();
            if (data == null) { return 0; }

            // LAST ENTRY TO KEEP DATE
            var lastEntryToKeepDate = await this.dbContext.Entries
               .Where(x => x.EntryID == lastEntryToKeepID)
               .Select(x => x.DueDate)
               .FirstOrDefaultAsync();

            // ENTRIES TO REMOVE
            var entriesToRemove = await this.dbContext.Entries
               .Where(x => x.RecurrencyID == recurrencyID)
               .Where(x => x.RowStatus == 1 && x.Paid == false && x.EntryID != lastEntryToKeepID && x.DueDate > lastEntryToKeepDate)
               .ToListAsync();
            if (entriesToRemove.Count == 0) { return 0; }

            // APPLY
            entriesToRemove.ForEach(x => x.RowStatus = -1);
            this.dbContext.RemoveRange(entriesToRemove);
            await this.dbContext.SaveChangesAsync();

            // RESULT
            return entriesToRemove.Count;
         }
         catch (Exception) { throw; }
      }

   }

}
