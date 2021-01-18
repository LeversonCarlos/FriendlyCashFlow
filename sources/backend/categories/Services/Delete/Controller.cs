using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Categories
{

   partial class CategoryController
   {

      [HttpDelete("delete/{id}")]
      public Task<IActionResult> DeleteAsync(string id) =>
         _CategoryService.DeleteAsync(id);

   }

}
