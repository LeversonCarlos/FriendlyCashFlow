using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Entries
{

   partial class EntriesService
   {

      internal async Task<ActionResult<bool>> RemoveAsync(long entryID, bool removeFutureRecurrencies)
      {
         try
         {

            // LOCATE DATA
            var data = await this.GetDataQuery().Where(x => x.EntryID == entryID).FirstOrDefaultAsync();
            if (data == null) { return this.NotFoundResponse(); }

            // REMOVE BALANCE
            await this.GetService<Balances.BalancesService>().RemoveAsync(data);

            // REMOVE PATTERN
            if (data.PatternID.HasValue)
            { await this.GetService<Patterns.PatternsService>().RemoveAsync(data.PatternID.Value); }

            // APPLY
            data.RowStatus = -1;
            this.dbContext.Remove(data);
            await this.dbContext.SaveChangesAsync();

            // FIX BALANCE
            await this.GetService<Balances.BalancesService>().FixAsync(data);

            // REMOVE RECURRENCY
            if (data.RecurrencyID.HasValue && data.RecurrencyID.Value > 0)
            {
               if (removeFutureRecurrencies)
               { await this.GetService<Recurrencies.RecurrenciesService>().RemoveEntriesAsync(data.RecurrencyID.Value, data.EntryID); }
               await this.GetService<Recurrencies.RecurrenciesService>().RemoveAsync(data.RecurrencyID.Value);
            }

            // RESULT
            return this.OkResponse(true);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

   partial class EntriesController
   {
      [HttpDelete("{id:long}/{removeFutureRecurrencies:bool}")]
      [HttpDelete("{id:long}")]
      [Authorize(Roles = "Editor")]
      public async Task<ActionResult<bool>> RemoveAsync(long id, bool removeFutureRecurrencies = false)
      {
         return await this.GetService<EntriesService>().RemoveAsync(id, removeFutureRecurrencies);
      }
   }

}
