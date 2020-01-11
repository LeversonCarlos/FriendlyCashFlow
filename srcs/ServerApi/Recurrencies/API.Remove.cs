using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Recurrencies
{

   partial class RecurrenciesService
   {

      public async Task<ActionResult<bool>> RemoveAsync(long recurrencyID)
      {
         try
         {

            // LOCATE ROW
            var data = await this.GetDataQuery().Where(x => x.RecurrencyID == recurrencyID).FirstOrDefaultAsync();
            if (data == null) { return this.NotFoundResponse(); }

            // CHECK FOR REMINING ENTRIES
            var remainingEntries = await this.dbContext.Entries.Where(x => x.RecurrencyID == recurrencyID).CountAsync();
            if (remainingEntries > 0) { return true; }

            // APPLY
            data.RowStatus = -1;
            this.dbContext.Remove(data);
            await this.dbContext.SaveChangesAsync();

            // RESULT
            return this.OkResponse(true);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

}
