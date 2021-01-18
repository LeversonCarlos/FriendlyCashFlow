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
            return new BadRequestObjectResult(new string[] { WARNINGS.INVALID_INSERT_PARAMETER });

         // VALIDATE PARENT
         // TODO

         // VALIDATE DUPLICITY
         var categoryList = await _CategoryRepository.SearchCategoriesAsync(insertVM.Type, insertVM.ParentID, insertVM.Text);
         if (categoryList != null && categoryList.Length > 0)
            return new BadRequestObjectResult(new string[] { WARNINGS.CATEGORY_TEXT_ALREADY_USED });

         // ADD NEW CATEGORY
         var category = new CategoryEntity(insertVM.Text, insertVM.Type, insertVM.ParentID);
         await _CategoryRepository.InsertCategoryAsync(category);

         // TRACK EVENT
         _InsightsService.TrackEvent("Category Service Insert");

         // RESULT
         return new OkResult();
      }

   }

   partial interface ICategoryService
   {
      Task<IActionResult> InsertAsync(InsertVM insertVM);
   }

   partial struct WARNINGS
   {
      internal const string INVALID_INSERT_PARAMETER = "INVALID_INSERT_PARAMETER";
      internal const string CATEGORY_TEXT_ALREADY_USED = "CATEGORY_TEXT_ALREADY_USED";
   }

}
