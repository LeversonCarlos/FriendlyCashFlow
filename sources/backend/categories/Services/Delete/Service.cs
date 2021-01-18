using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Categories
{

   partial class CategoryService
   {

      public async Task<IActionResult> DeleteAsync(string id)
      {

         // VALIDATE PARAMETERS
         if (!Shared.EntityID.TryParse(id, out var categoryID))
            return new BadRequestObjectResult(new string[] { WARNINGS.INVALID_DELETE_PARAMETER });

         // LOCATE CATEGORY
         var category = (CategoryEntity)(await _CategoryRepository.LoadCategoryAsync(categoryID));
         if (category == null)
            return new BadRequestObjectResult(new string[] { WARNINGS.CATEGORY_NOT_FOUND });

         // REMOVE CATEGORY
         await _CategoryRepository.DeleteCategoryAsync(categoryID);

         // TRACK EVENT
         _InsightsService.TrackEvent("Category Service Delete");

         // RESULT
         return new OkResult();
      }

   }

   partial interface ICategoryService
   {
      Task<IActionResult> DeleteAsync(string id);
   }

   partial struct WARNINGS
   {
      internal const string INVALID_DELETE_PARAMETER = "INVALID_CATEGORYID_PARAMETER";
   }

}
