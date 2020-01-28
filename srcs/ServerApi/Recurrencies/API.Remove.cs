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

   }

}
