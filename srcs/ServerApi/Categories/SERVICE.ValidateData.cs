
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Categories
{
   partial class CategoriesService
   {

      private async Task<ActionResult<bool>> ValidateDataAsync(CategoryVM value)
      {
         try
         {

            // VALIDATE DUPLICITY
            if (await this.GetDataQuery().CountAsync(x => x.CategoryID != value.CategoryID && x.Text == value.Text) != 0)
            { return this.WarningResponse(this.GetTranslation("CATEGORIES_CATEGORY_TEXT_ALREADY_EXISTS_WARNING")); }

            // RESULT
            return this.OkResponse(true);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }
}
