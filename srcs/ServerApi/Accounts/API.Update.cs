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

      public async Task<ActionResult<AccountVM>> UpdateAsync(long accountID, AccountVM value)
      {
         try
         {

            // VALIDATE
            var validateMessage = await this.ValidateAsync(value);
            var validateResult = this.GetValue(validateMessage);
            if (!validateResult) { return validateMessage.Result; }

            // LOCATE DATA
            var data = await this.GetDataQuery().Where(x => x.AccountID == accountID).FirstOrDefaultAsync();
            if (data == null) { return this.NotFoundResponse(); }

            // APPLY
            data.Text = value.Text;
            data.Type = (short)value.Type;
            data.ClosingDay = value.ClosingDay;
            data.DueDay = value.DueDay;
            data.Active = value.Active;
            await this.dbContext.SaveChangesAsync();

            // RESULT
            var result = AccountVM.Convert(data);
            return this.OkResponse(result);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

   partial class AccountController
   {
      [HttpPut("{id:long}")]
      [Authorize(Roles = "Editor")]
      public async Task<ActionResult<AccountVM>> UpdateAsync(long id, [FromBody]AccountVM value)
      {
         if (value == null) { return this.BadRequest(this.ModelState); }
         return await this.GetService<AccountsService>().UpdateAsync(id, value);
      }
   }

}
