using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Import
{
   partial class ImportService
   {

      private async Task<ActionResult<bool>> ValidateAsync(ImportVM value)
      {
         try
         {

            // VALIDATE TYPE
            if (value.Entries.Any(x => x.Type != Categories.enCategoryType.Income && x.Type != Categories.enCategoryType.Expense))
            { return this.WarningResponse(this.GetTranslation("IMPORT_INVALID_TYPE_WARNING")); }

            // VALIDATE TEXT
            if (value.Entries.Any(x => string.IsNullOrEmpty(x.Text)))
            { return this.WarningResponse(this.GetTranslation("IMPORT_INVALID_TEXT_WARNING")); }

            // VALIDATE DATES
            if (value.Entries.Any(x => x.DueDate == DateTime.MinValue))
            { return this.WarningResponse(this.GetTranslation("IMPORT_INVALID_DUEDATE_WARNING")); }
            if (value.Entries.Any(x => x.Paid && (!x.PayDate.HasValue || x.PayDate.Value == null || x.PayDate.Value == DateTime.MinValue)))
            { return this.WarningResponse(this.GetTranslation("IMPORT_INVALID_PAYDATE_WARNING")); }

            // VALIDATE VALUE
            if (value.Entries.Any(x => Math.Round(x.Value, 2) == 0))
            { return this.WarningResponse(this.GetTranslation("IMPORT_INVALID_VALUE_WARNING")); }

            // VALIDATE ACCOUNT
            if (value.Entries.Any(x => string.IsNullOrEmpty(x.Account)))
            { return this.WarningResponse(this.GetTranslation("IMPORT_INVALID_ACCOUNT_WARNING")); }

            // VALIDATE CATEGORY
            if (value.Entries.Any(x => string.IsNullOrEmpty(x.Category)))
            { return this.WarningResponse(this.GetTranslation("IMPORT_INVALID_CATEGORY_WARNING")); }

            // RESULT
            return this.OkResponse(await Task.FromResult(true));
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }
}
