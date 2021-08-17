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

      internal async Task<ActionResult<BudgetVM>> CreateAsync(BudgetVM value)
      {
         try
         {

            // VALIDATE
            var validateMessage = await this.ValidateAsync(value);
            var validateResult = this.GetValue(validateMessage);
            if (!validateResult)
               return validateMessage.Result;

            // NEW MODEL
            var data = new BudgetData()
            {
               ResourceID = this.GetService<Helpers.User>().ResourceID,
               PatternID = value.PatternRow.PatternID,
               Value = Math.Abs(value.Value),
               RowStatus = 1
            };

            // APPLY
            await this.dbContext.Budget.AddAsync(data);
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

      [HttpPost("")]
      [Authorize(Roles = "Editor")]
      public async Task<ActionResult<BudgetVM>> CreateAsync([FromBody] BudgetVM value)
      {
         if (value == null) { return this.BadRequest(this.ModelState); }
         return await this.GetService<BudgetService>().CreateAsync(value);
      }

   }

}
