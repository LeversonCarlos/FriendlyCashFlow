using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Transfers
{
   public interface ITransferService
   {

      Task<ActionResult<ITransferEntity[]>> ListAsync(int year, int month);
      Task<ActionResult<ITransferEntity>> LoadAsync(string id);

      Task<IActionResult> InsertAsync(InsertVM insertVM);
      Task<IActionResult> UpdateAsync(UpdateVM updateVM);
      Task<IActionResult> DeleteAsync(string id);

   }
}
