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

      internal async Task<ActionResult<CategoryVM>> CreateAsync(CategoryVM value)
      {
         try
         {

            // VALIDATE
            var validateMessage = await this.ValidateAsync(value);
            var validateResult = this.GetValue(validateMessage);
            if (!validateResult) { return validateMessage.Result; }

            // NEW MODEL
            var user = this.GetService<Helpers.User>();
            var data = new CategoryData()
            {
               ResourceID = user.ResourceID,
               Text = value.Text,
               Type = (short)value.Type,
               ParentID = value.ParentID,
               RowStatus = 1
            };

            // HIERARCHY TEXT
            var parentRow = await this.GetDataQuery().Where(x => x.CategoryID == value.ParentID).FirstOrDefaultAsync();
            data.HierarchyText = string.Empty;
            if (parentRow != null)
            { data.HierarchyText = $"{parentRow.HierarchyText} / "; }
            data.HierarchyText += value.Text;

            // APPLY
            await this.dbContext.Categories.AddAsync(data);
            await this.dbContext.SaveChangesAsync();

            // RESULT
            var result = CategoryVM.Convert(data, parentRow);
            return this.CreatedResponse("categories", result.CategoryID, result);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

   partial class CategoriesController
   {
      [HttpPost("")]
      [Authorize(Roles = "Editor")]
      public async Task<ActionResult<CategoryVM>> CreateAsync([FromBody]CategoryVM value)
      {
         if (value == null) { return this.BadRequest(this.ModelState); }
         return await this.GetService<CategoriesService>().CreateAsync(value);
      }
   }

}
