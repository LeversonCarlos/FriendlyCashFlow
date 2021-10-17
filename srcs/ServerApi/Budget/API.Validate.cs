using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Budget
{
   partial class BudgetService
   {

      private async Task<ActionResult<bool>> ValidateAsync(BudgetVM value)
      {
         try
         {

            // VALIDATE VALUE
            if (Math.Round(value.Value, 2) == 0)
            { return this.WarningResponse(this.GetTranslation("BUDGET_VALUE_INVALID_WARNING")); }

            // VALIDATE PATTERN
            if (value.PatternRow == null)
               return this.WarningResponse(this.GetTranslation("BUDGET_PATTERN_NOT_FOUND_WARNING"));
            var patternFound = await this.GetService<Patterns.PatternsService>()
               .GetDataQuery()
               .Where(x => x.PatternID == value.PatternRow.PatternID)
               .AnyAsync();
            if (!patternFound)
               return this.WarningResponse(this.GetTranslation("BUDGET_PATTERN_NOT_FOUND_WARNING"));

            // RESULT
            return this.OkResponse(true);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }
}
