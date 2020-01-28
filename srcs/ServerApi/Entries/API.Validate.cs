using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Entries
{
   partial class EntriesService
   {

      private async Task<ActionResult<bool>> ValidateAsync(EntryVM value)
      {
         try
         {

            // VALIDATE DATES
            if (value.DueDate == null || value.DueDate == DateTime.MinValue)
            { return this.WarningResponse(this.GetTranslation("ENTRIES_DUEDATE_INVALID_WARNING")); }
            if (value.Paid && (!value.PayDate.HasValue || value.PayDate.Value == null || value.PayDate.Value == DateTime.MinValue))
            { return this.WarningResponse(this.GetTranslation("ENTRIES_PAYDATE_INVALID_WARNING")); }

            // VALIDATE VALUE
            if (Math.Round(value.EntryValue, 2) == 0)
            { return this.WarningResponse(this.GetTranslation("ENTRIES_ENTRYVALUE_INVALID_WARNING")); }

            // VALIDATE CONTEXT
            if (value.CategoryID.HasValue)
            {
               var categoryFound = await this.GetService<Categories.CategoriesService>().GetDataQuery().Where(x => x.CategoryID == value.CategoryID.Value).AnyAsync();
               if (!categoryFound) { return this.WarningResponse(this.GetTranslation("ENTRIES_CATEGORY_NOT_FOUND_WARNING")); }
            }

            // VALIDATE ACCOUNT
            var accountFound = await this.GetService<Accounts.AccountsService>().GetDataQuery().Where(x => x.AccountID == value.AccountID).AnyAsync();
            if (!accountFound) { return this.WarningResponse(this.GetTranslation("ENTRIES_ACCOUNT_NOT_FOUND_WARNING")); }

            // RESULT
            return this.OkResponse(true);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }
}
