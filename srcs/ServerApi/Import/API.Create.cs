using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Import
{

   partial class ImportService
   {

      internal async Task<ActionResult<bool>> CreateAsync(ImportVM value)
      {
         try
         {

            // VALIDATE
            var validateMessage = await this.ValidateAsync(value);
            var validateResult = this.GetValue(validateMessage);
            if (!validateResult) { return validateMessage.Result; }

            // USER
            value.ResourceID = this.GetService<Helpers.User>().ResourceID;

            // CLEAR
            await this.ClearAsync(value);

            // ACCOUNTS
            var accountsMessage = await this.CreateAccountsAsync(value);
            var accountsResult = this.GetValue(accountsMessage);
            if (!accountsResult) { return accountsMessage.Result; }

            // CATEGORIES
            // TODO

            // ENTRIES
            // TODO

            // TRANSFERS
            // TODO

            // RESULT
            return this.OkResponse(true);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

   partial class ImportController
   {

      [HttpPost("")]
      [Authorize(Roles = "Editor")]
      public async Task<ActionResult<bool>> CreateAsync([FromBody]ImportVM value)
      {
         if (value == null) { return this.BadRequest(this.ModelState); }
         return await this.GetService<ImportService>().CreateAsync(value);
      }

   }

}
