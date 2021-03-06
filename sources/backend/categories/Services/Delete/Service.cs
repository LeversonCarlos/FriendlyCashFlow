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
            return Warning(WARNINGS.INVALID_DELETE_PARAMETER);

         // LOCATE CATEGORY
         var category = await _CategoryRepository.LoadAsync(categoryID);
         if (category == null)
            return Warning(WARNINGS.CATEGORY_NOT_FOUND);

         // VALIDATE IF CATEGORY HAS NO CHILDREN
         var children = await _CategoryRepository.SearchAsync(category.Type, category.CategoryID, "");
         if (children.Length > 0)
            return Warning(WARNINGS.STILL_HAS_CHILDREN_CANT_REMOVE);

         // REMOVE CATEGORY
         await _CategoryRepository.DeleteAsync(categoryID);

         // TRACK EVENT
         _InsightsService.TrackEvent("Category Service Delete");

         // RESULT
         return Ok();
      }

   }
}
