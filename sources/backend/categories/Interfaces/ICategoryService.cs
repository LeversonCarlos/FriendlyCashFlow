using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Categories
{
   public interface ICategoryService
   {

      Task<ActionResult<ICategoryEntity[]>> ListAsync();
      Task<ActionResult<ICategoryEntity>> LoadAsync(string id);

      Task<IActionResult> InsertAsync(InsertVM insertVM);
      Task<IActionResult> UpdateAsync(UpdateVM updateVM);
      Task<IActionResult> DeleteAsync(string id);

   }
}
