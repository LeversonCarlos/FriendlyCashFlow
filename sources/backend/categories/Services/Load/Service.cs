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
            return new BadRequestObjectResult(new string[] { WARNINGS.INVALID_LOAD_PARAMETER });

         // LOAD CATEGORY
         var category = await _CategoryRepository.LoadCategoryAsync(categoryID);

         // RESULT
         return new OkObjectResult(category);
      }

   }

   partial interface ICategoryService
   {
      Task<ActionResult<ICategoryEntity>> LoadAsync(string id);
   }

   partial struct WARNINGS
   {
      internal const string INVALID_LOAD_PARAMETER = "INVALID_LOAD_PARAMETER";
   }

}
