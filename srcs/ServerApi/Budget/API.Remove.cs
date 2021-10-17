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

      internal async Task<ActionResult<bool>> RemoveAsync(long budgetID)
      {
         try
         {

            // LOCATE DATA
            var data = await this
               .GetDataQuery()
               .Where(x => x.BudgetID == budgetID)
               .FirstOrDefaultAsync();
            if (data == null)
               return this.NotFoundResponse();

            // APPLY
            data.RowStatus = -1;
            this.dbContext.Remove(data);
            await this.dbContext.SaveChangesAsync();

            // RESULT
            return this.OkResponse(true);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

   partial class BudgetController
   {
      [HttpDelete("{id:long}")]
      [Authorize(Roles = "Editor")]
      public async Task<ActionResult<bool>> RemoveAsync(long id)
      {
         return await this.GetService<BudgetService>().RemoveAsync(id);
      }
   }

}
