
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

      private async Task<ActionResult<bool>> ValidateDataAsync(AccountVM value)
      {
         try
         {

            // VALIDATE DUPLICITY
            if (await this.GetDataQuery().CountAsync(x => x.AccountID != value.AccountID && x.Text == value.Text) != 0)
            { return this.WarningResponse(this.GetTranslation("ACCOUNTS_ACCOUNT_TEXT_ALREADY_EXISTS_WARNING")); }

            // VALIDATE CREDIT CARD
            if (value.Type == enAccountType.CreditCard)
            {
               if (!value.DueDay.HasValue || value.DueDay <= 0 || value.DueDay >= 31)
               { return this.WarningResponse(this.GetTranslation("ACCOUNTS_INVALID_DUE_DAY_WARNING")); }
            }

            // RESULT
            return this.OkResponse(true);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }
}
