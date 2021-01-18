using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Categories
{

   partial class CategoryController
   {

      [HttpPut("update")]
      public Task<IActionResult> UpdateAsync([FromBody] UpdateVM updateVM) =>
         _CategoryService.UpdateAsync(updateVM);

   }

}
