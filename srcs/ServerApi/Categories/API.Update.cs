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

      public async Task<ActionResult<CategoryVM>> UpdateAsync(long categoryID, CategoryVM value)
      {
         try
         {

            // VALIDATE
            var validateMessage = await this.ValidateAsync(value);
            var validateResult = this.GetValue(validateMessage);
            if (!validateResult) { return validateMessage.Result; }

            // LOCATE DATA
            var data = await this.GetDataQuery().Where(x => x.CategoryID == categoryID).FirstOrDefaultAsync();
            if (data == null) { return this.NotFoundResponse(); }

            // HIERARCHY TEXT
            var parentRow = await this.GetDataQuery().Where(x => x.CategoryID == value.ParentID).FirstOrDefaultAsync();
            data.HierarchyText = string.Empty;
            if (parentRow != null)
            { data.HierarchyText = $"{parentRow.HierarchyText} / "; }
            data.HierarchyText += value.Text;

            // UPDATE CHILDREN
            await this.UpdateAsync_Children(data.CategoryID, data.HierarchyText);

            // APPLY
            data.Text = value.Text;
            data.ParentID = value.ParentID;
            await this.dbContext.SaveChangesAsync();

            // RESULT
            var result = CategoryVM.Convert(data, parentRow);
            return this.OkResponse(result);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

      private async Task UpdateAsync_Children(long parentID, string parentText)
      {
         var children = await this.GetDataQuery().Where(x => x.ParentID == parentID).ToListAsync();
         foreach (var child in children)
         {
            child.HierarchyText = $"{parentText} / {child.Text}";
            await this.UpdateAsync_Children(child.CategoryID, child.HierarchyText);
         }
      }

   }

   partial class CategoriesController
   {
      [HttpPut("{id:long}")]
      [Authorize(Roles = "Editor")]
      public async Task<ActionResult<CategoryVM>> UpdateAsync(long id, [FromBody]CategoryVM value)
      {
         if (value == null) { return this.BadRequest(this.ModelState); }
         return await this.GetService<CategoriesService>().UpdateAsync(id, value);
      }
   }

}
