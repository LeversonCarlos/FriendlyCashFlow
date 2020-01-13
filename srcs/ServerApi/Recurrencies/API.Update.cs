using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Recurrencies
{

   partial class RecurrenciesService
   {

      internal async Task<bool> UpdateAsync(long recurrencyID, long editedEntryID)
      {
         try
         {

            // LOCATE ROW
            var data = await this.GetDataQuery().Where(x => x.RecurrencyID == recurrencyID).FirstOrDefaultAsync();
            if (data == null) { return false; }

            // REMOVE FUTURE ENTRIES
            var removedEntries = await this.RemoveFutureAsync(recurrencyID, editedEntryID);
            if (removedEntries == 0) { return true; }

            // LAST ENTRY TO KEEP DATE
            var editedEntry = await this.dbContext.Entries
               .Where(x => x.EntryID == editedEntryID)
               .FirstOrDefaultAsync();

            // UPDATE RECURRENCY DATA
            data.PatternID = editedEntry.PatternID.Value;
            data.AccountID = editedEntry.AccountID.Value;
            data.InitialDate = editedEntry.DueDate;
            data.EntryDate = editedEntry.DueDate;
            data.EntryValue = editedEntry.EntryValue;
            data.Count = removedEntries;
            await this.dbContext.SaveChangesAsync();

            // GENERATE
            await this.GenerateRecurrencyAsync(recurrencyID);

            // RESULT
            return true;
         }
         catch (Exception) { throw; }
      }

   }

}
