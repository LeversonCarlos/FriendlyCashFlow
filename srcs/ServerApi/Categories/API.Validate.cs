using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Categories
{
   partial class CategoriesService
   {

      private async Task<ActionResult<bool>> ValidateAsync(CategoryVM value)
      {
         try
         {

            // VALIDATE DUPLICITY
            if (await this.GetDataQuery().CountAsync(x => x.ParentID == value.ParentID && x.CategoryID != value.CategoryID && x.Text == value.Text) != 0)
            { return this.WarningResponse(this.GetTranslation("CATEGORIES_CATEGORY_TEXT_ALREADY_EXISTS_WARNING")); }

            // CHECK HIERARCHY BREAK
            if (value.CategoryID > 0)
            {
               var hierarchyText = await this.GetDataQuery().Where(x => x.CategoryID == value.CategoryID).Select(x => x.HierarchyText).FirstOrDefaultAsync();

               // CHECK IF WE ARE DEFINING ANY CHILD AS A PARENT
               var foundChildren = await this.GetDataQuery()
                  .Where(x => x.CategoryID == value.ParentID && x.HierarchyText.Contains(hierarchyText))
                  .AnyAsync();
               if (foundChildren) { return this.WarningResponse(this.GetTranslation("CATEGORIES_CHILDREN_CANT_BE_DEFINED_AS_PARENT_CATEGORY_WARNING")); }

               // CHECK IF WE ARE DEFINING A PARENT AS A CHLID
               /*
               var foundParent = await this.GetDataQuery()
                  .Where(x => x.CategoryID == value.ParentID && hierarchyText.Contains(x.HierarchyText))
                  .AnyAsync();
               if (foundChildren) { return this.WarningResponse(this.GetTranslation("CATEGORIES_PARENT_CANT_BE_DEFINED_AS_CHILD_CATEGORY_WARNING")); }
               */

            }

            // RESULT
            return this.OkResponse(true);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }
}
