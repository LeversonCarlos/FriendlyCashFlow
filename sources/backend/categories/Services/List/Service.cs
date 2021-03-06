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
         var categoriesList = await _CategoryRepository.ListAsync();

         // RESULT
         return Ok(categoriesList);
      }

   }
}
