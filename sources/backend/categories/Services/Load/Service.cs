using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Categories
{

   partial class CategoryService
   {

      public async Task<ActionResult<ICategoryEntity>> LoadAsync(string id)
      {

         // VALIDATE PARAMETERS
         if (!Shared.EntityID.TryParse(id, out var categoryID))
            return Warning(WARNINGS.INVALID_LOAD_PARAMETER);

         // LOAD CATEGORY
         var category = await _CategoryRepository.LoadCategoryAsync(categoryID);

         // RESULT
         return Ok(category);
      }

   }

   partial struct WARNINGS
   {
      internal const string INVALID_LOAD_PARAMETER = "INVALID_CATEGORYID_PARAMETER";
   }

}
