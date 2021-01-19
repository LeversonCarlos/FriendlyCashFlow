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
            return Shared.Results.Warning(WARNINGS.INVALID_DELETE_PARAMETER);

         // LOCATE CATEGORY
         var category = (CategoryEntity)(await _CategoryRepository.LoadCategoryAsync(categoryID));
         if (category == null)
            return Shared.Results.Warning(WARNINGS.CATEGORY_NOT_FOUND);

         // REMOVE CATEGORY
         await _CategoryRepository.DeleteCategoryAsync(categoryID);

         // TRACK EVENT
         _InsightsService.TrackEvent("Category Service Delete");

         // RESULT
         return Shared.Results.Ok();
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
