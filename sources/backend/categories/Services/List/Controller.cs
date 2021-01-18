using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Categories
{

   partial class CategoryController
   {

      [HttpGet("list")]
      public Task<ActionResult<ICategoryEntity[]>> ListAsync() =>
         _CategoryService.ListAsync();

   }

}
