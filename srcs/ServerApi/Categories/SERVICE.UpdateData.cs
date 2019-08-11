
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Categories
{
   partial class CategoriesService
   {

      public async Task<ActionResult<CategoryVM>> UpdateDataAsync(long categoryID, CategoryVM value)
      {
         try
         {

            // VALIDATE
            var validateMessage = await this.ValidateDataAsync(value);
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

   }
}
