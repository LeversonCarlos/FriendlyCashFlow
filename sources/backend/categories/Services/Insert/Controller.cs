using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Categories
{

   partial class CategoryController
   {

      [HttpPost("insert")]
      public Task<IActionResult> InsertAsync([FromBody] InsertVM insertVM) =>
         _CategoryService.InsertAsync(insertVM);

   }

}
