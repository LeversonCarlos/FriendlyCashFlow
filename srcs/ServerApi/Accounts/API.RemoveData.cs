using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Accounts
{

   partial class AccountsService
   {

      public async Task<ActionResult<bool>> RemoveDataAsync(long accountID)
      {
         try
         {

            // LOCATE DATA
            var data = await this.GetDataQuery().Where(x => x.AccountID == accountID).FirstOrDefaultAsync();
            if (data == null) { return this.NotFoundResponse(); }

            // APPLY
            data.RowStatus = -1;
            await this.dbContext.SaveChangesAsync();

            // RESULT
            return this.OkResponse(true);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

   partial class AccountController
   {
      [HttpDelete("{id:long}")]
      [Authorize(Roles = "Editor")]
      public async Task<ActionResult<bool>> RemoveDataAsync(long id)
      {
         var service = this.GetService<AccountsService>();
         return await service.RemoveDataAsync(id);
      }
   }

}
