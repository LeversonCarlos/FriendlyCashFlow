using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace Elesse.Categories
{

   partial class CategoryService
   {

      public async Task<IActionResult> UpdateAsync(UpdateVM updateVM)
      {

         // VALIDATE PARAMETERS
         if (updateVM == null)
            return Warning(WARNINGS.INVALID_UPDATE_PARAMETER);

         // VALIDATE PARENT
         if (updateVM.ParentID != null)
         {
            var parent = await _CategoryRepository.LoadCategoryAsync(updateVM.ParentID);
            if (parent == null)
               return Warning(WARNINGS.PARENT_CATEGORY_NOT_FOUND);
         }

         // VALIDATE DUPLICITY
         var categoriesList = await _CategoryRepository.SearchCategoriesAsync(updateVM.Type, updateVM.ParentID, updateVM.Text);
         if (categoriesList.Any(x => x.CategoryID != updateVM.CategoryID))
            return Warning(WARNINGS.CATEGORY_TEXT_ALREADY_USED);

         // LOCATE CATEGORY
         var category = (CategoryEntity)(await _CategoryRepository.LoadCategoryAsync(updateVM.CategoryID));
         if (category == null)
            return Warning(WARNINGS.CATEGORY_NOT_FOUND);

         // APPLY CHANGES
         category.Text = updateVM.Text;
         category.Type = updateVM.Type;
         category.ParentID = updateVM.ParentID;

         // SAVE CHANGES
         await _CategoryRepository.UpdateCategoryAsync(category);

         // RESULT
         return Ok();
      }

   }

   partial struct WARNINGS
   {
      internal const string INVALID_UPDATE_PARAMETER = "INVALID_UPDATE_PARAMETER";
      internal const string CATEGORY_NOT_FOUND = "CATEGORY_NOT_FOUND";
   }

}
