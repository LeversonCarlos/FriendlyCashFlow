
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Accounts
{
   partial class AccountsService
   {

      public async Task<ActionResult<AccountVM>> UpdateDataAsync(long accountID, AccountVM value)
      {
         try
         {

            // VALIDATE
            var validateMessage = await this.ValidateDataAsync(value);
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
}
