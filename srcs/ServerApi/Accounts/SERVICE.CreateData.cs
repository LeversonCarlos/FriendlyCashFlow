
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

      public async Task<ActionResult<AccountVM>> CreateDataAsync(AccountVM value)
      {
         try
         {

            // VALIDATE
            var validateMessage = await this.ValidateDataAsync(value);
            if (!validateMessage.Value) { return validateMessage.Result; }

            // NEW MODEL
            var data = new AccountData()
            {
               ResourceID = resourceID,
               Text = value.Text,
               Type = (short)value.Type,
               Active = value.Active,
               // ClosingDay = value.ClosingDay,
               DueDay = value.DueDay,
               RowStatus = 1
            };

            // APPLY
            await this.dbContext.Accounts.AddAsync(data);
            await this.dbContext.SaveChangesAsync();

            // RESULT
            var result = AccountVM.Convert(data);
            return this.CreatedResponse("accounts", result.AccountID, result);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }
}
