using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Elesse.Categories
{

   partial class CategoryService
   {

      public async Task<IActionResult> InsertAsync(InsertVM insertVM)
      {

         // VALIDATE PARAMETERS
         if (insertVM == null)
            return Warning(WARNINGS.INVALID_INSERT_PARAMETER);

         // VALIDATE PARENT
         if (insertVM.ParentID != null)
         {
            var parent = await _CategoryRepository.LoadCategoryAsync(insertVM.ParentID);
            if (parent == null)
               return Warning(WARNINGS.PARENT_CATEGORY_NOT_FOUND);
         }

         // VALIDATE DUPLICITY
         var categoryList = await _CategoryRepository.SearchCategoriesAsync(insertVM.Type, insertVM.ParentID, insertVM.Text);
         if (categoryList != null && categoryList.Length > 0)
            return Warning(WARNINGS.CATEGORY_TEXT_ALREADY_USED);

         // ADD NEW CATEGORY
         var category = new CategoryEntity(insertVM.Text, insertVM.Type, insertVM.ParentID);
         await _CategoryRepository.InsertCategoryAsync(category);

         // TRACK EVENT
         _InsightsService.TrackEvent("Category Service Insert");

         // RESULT
         return Ok();
      }

   }

   partial struct WARNINGS
   {
      internal const string INVALID_INSERT_PARAMETER = "INVALID_INSERT_PARAMETER";
      internal const string CATEGORY_TEXT_ALREADY_USED = "CATEGORY_TEXT_ALREADY_USED";
      internal const string PARENT_CATEGORY_NOT_FOUND = "PARENT_CATEGORY_NOT_FOUND";
   }

}
