using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Transfers
{

   partial class TransfersService
   {

      internal async Task<ActionResult<bool>> RemoveAsync(string transferID)
      {
         try
         {

            // LOCATE DATA
            var entries = await this.GetDataQuery()
               .Where(x => x.TransferID == transferID)
               .ToListAsync();
            if (entries == null || entries.Count != 2) { return this.WarningResponse("TRANSFERS_RECORD_NOT_FOUND_WARNING"); }

            // APPLY
            await this.GetService<Entries.EntriesService>().RemoveAsync(entries[0].EntryID);
            await this.GetService<Entries.EntriesService>().RemoveAsync(entries[1].EntryID);

            // RESULT
            return this.OkResponse(true);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

   partial class TransfersController
   {
      [HttpDelete("{id}")]
      [Authorize(Roles = "Editor")]
      public async Task<ActionResult<bool>> RemoveAsync(string id)
      {
         return await this.GetService<TransfersService>().RemoveAsync(id);
      }
   }

}
