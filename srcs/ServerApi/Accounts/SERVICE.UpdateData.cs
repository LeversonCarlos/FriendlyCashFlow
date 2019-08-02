
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

            // VALIDATE DUPLICITY
            if (this.GetDataQuery().Count(x => x.AccountID != accountID && x.Text == value.Text) != 0)
            { return this.WarningResponse("A account with this text already exists"); }

            // VALIDATE CREDIT CARD
            if (value.Type == enAccountType.CreditCard)
            {
               if (!value.DueDay.HasValue || value.DueDay <= 0 || value.DueDay >= 31)
               { return this.WarningResponse("A valid due day must be informed"); }
            }

            // LOCATE DATA
            var data = await this.GetDataQuery().Where(x => x.AccountID == accountID).FirstOrDefaultAsync();
            if (data == null) { return this.NotFoundResponse(); }

            // APPLY
            data.Text = value.Text;
            data.Type = (short)value.Type;
            // data.ClosingDay = value.ClosingDay;
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
