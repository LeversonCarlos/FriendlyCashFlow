using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace Elesse.Categories
{

   partial class CategoryService
   {

      public async Task<ActionResult<ICategoryEntity[]>> ListAsync()
      {

         // LOAD CATEGORIES
         var categoriesList = await _CategoryRepository.ListCategoriesAsync();

         // RESULT
         return new OkObjectResult(categoriesList);
      }

   }

   partial interface ICategoryService
   {
      Task<ActionResult<ICategoryEntity[]>> ListAsync();
   }

}
