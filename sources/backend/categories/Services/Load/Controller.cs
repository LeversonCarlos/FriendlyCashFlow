using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Categories
{

   partial class CategoryController
   {

      [HttpGet("load/{id}")]
      public Task<ActionResult<ICategoryEntity>> LoadAsync(string id) =>
         _CategoryService.LoadAsync(id);

   }

}
