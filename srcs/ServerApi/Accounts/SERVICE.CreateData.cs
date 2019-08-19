
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Accounts
{
   partial class AccountsService
   {

      public async Task<ActionResult<AccountVM>> CreateDataAsync(AccountVM value)
      {
         try
         {
            var user = this.GetService<Helpers.User>();

            // VALIDATE
            var validateMessage = await this.ValidateDataAsync(value);
            var validateResult = this.GetValue(validateMessage);
            if (!validateResult) { return validateMessage.Result; }

            // NEW MODEL
            var data = new AccountData()
            {
               ResourceID = user.UserID,
               Text = value.Text,
               Type = (short)value.Type,
               Active = value.Active,
               ClosingDay = value.ClosingDay,
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
