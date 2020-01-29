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

            // INITIALIZE
            var user = this.GetService<Helpers.User>();
            value.ResourceID = user.ResourceID;
            if (value.ClearDataBefore) { await this.ClearAsync(value); }
            this.TrackEvent("Import Data - Start",
               $"UserID:{user.UserID}",
               $"ClearDataBefore:{value.ClearDataBefore}",
               $"Entries:{value.Entries?.Count ?? 0}",
               $"Transfers:{value.Transfers?.Count ?? 0}");

            // ACCOUNTS
            var accountsMessage = await this.CreateAccountsAsync(value);
            var accountsResult = this.GetValue(accountsMessage);
            if (!accountsResult) { return accountsMessage.Result; }
            this.TrackEvent("Import Data - Accounts", $"UserID:{user.UserID}");

            // CATEGORIES
            var categoriesMessage = await this.CreateCategoriesAsync(value);
            var categoriesResult = this.GetValue(categoriesMessage);
            if (!categoriesResult) { return categoriesMessage.Result; }
            this.TrackEvent("Import Data - Categories", $"UserID:{user.UserID}");

            // ENTRIES
            var entriesMessage = await this.CreateEntriesAsync(value);
            var entriesResult = this.GetValue(entriesMessage);
            if (!entriesResult) { return entriesMessage.Result; }
            this.TrackEvent("Import Data - Entries", $"UserID:{user.UserID}");

            // TRANSFERS
            // TODO

            // RESULT
            this.TrackEvent("Import Data - Finish", $"UserID:{user.UserID}");
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
