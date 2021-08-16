using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Budget
{

   partial class BudgetService
   {

      internal async Task<ActionResult<BudgetVM>> UpdateAsync(long budgetID, BudgetVM viewModel)
      {
         try
         {

            // VALIDATE
            var validateMessage = await this.ValidateAsync(viewModel);
            var validateResult = this.GetValue(validateMessage);
            if (!validateResult)
               return validateMessage.Result;

            // LOCATE DATA
            var data = await this
               .GetDataQuery()
               .Where(x => x.BudgetID == budgetID)
               .FirstOrDefaultAsync();
            if (data == null)
               return this.NotFoundResponse();

            // APPLY CHANGES
            data.PatternID = viewModel.PatternRow.PatternID;
            data.Value = Math.Abs(viewModel.Value);

            // SAVE IT
            await this.dbContext.SaveChangesAsync();

            // RESULT
            var result = await GetDataAsync(data.BudgetID);
            return result;
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

   partial class BudgetController
   {
      [HttpPut("{id:long}")]
      [Authorize(Roles = "Editor")]
      public async Task<ActionResult<BudgetVM>> UpdateAsync(long id, [FromBody] BudgetVM value)
      {
         if (value == null) { return this.BadRequest(this.ModelState); }
         return await this.GetService<BudgetService>().UpdateAsync(id, value);
      }
   }

}
