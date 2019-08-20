using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Categories
{

   partial class CategoriesService
   {

      public async Task<ActionResult<bool>> RemoveDataAsync(long categoryID)
      {
         try
         {

            // LOCATE DATA
            var data = await this.GetDataQuery().Where(x => x.CategoryID == categoryID).FirstOrDefaultAsync();
            if (data == null) { return this.NotFoundResponse(); }

            // APPLY
            data.RowStatus = -1;
            await this.dbContext.SaveChangesAsync();

            // RESULT
            return this.OkResponse(true);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

   partial class CategoriesController
   {
      [HttpDelete("{id:long}")]
      [Authorize(Roles = "Editor")]
      public async Task<ActionResult<bool>> RemoveDataAsync(long id)
      {
         using (var service = new CategoriesService(this.serviceProvider))
         { return await service.RemoveDataAsync(id); }
      }
   }

}
