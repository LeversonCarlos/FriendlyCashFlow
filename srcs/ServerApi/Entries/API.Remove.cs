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

      public async Task<ActionResult<bool>> RemoveAsync(long entryID)
      {
         try
         {

            // LOCATE DATA
            var data = await this.GetDataQuery().Where(x => x.EntryID == entryID).FirstOrDefaultAsync();
            if (data == null) { return this.NotFoundResponse(); }

            // REMOVE BALANCE
            await this.GetService<Balances.BalancesService>().RemoveBalanceAsync(data);

            // REMOVE PATTERN
            await this.GetService<Patterns.PatternsService>().RemovePatternAsync(data.PatternID);

            // APPLY
            data.RowStatus = -1;
            this.dbContext.Remove(data);
            await this.dbContext.SaveChangesAsync();

            // REMOVE PATTERN
            if (data.RecurrencyID.HasValue && data.RecurrencyID.Value > 0)
            { await this.GetService<Recurrencies.RecurrenciesService>().RemoveAsync(data.RecurrencyID.Value); }

            // RESULT
            return this.OkResponse(true);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

   partial class EntriesController
   {
      [HttpDelete("{id:long}")]
      [Authorize(Roles = "Editor")]
      public async Task<ActionResult<bool>> RemoveAsync(long id)
      {
         return await this.GetService<EntriesService>().RemoveAsync(id);
      }
   }

}
