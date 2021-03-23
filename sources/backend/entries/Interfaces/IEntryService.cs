using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Entries
{
   public interface IEntryService
   {

      Task<ActionResult<IEntryEntity[]>> ListAsync(int year, int month);
      Task<ActionResult<IEntryEntity>> LoadAsync(string id);

      Task<IActionResult> InsertAsync(InsertVM insertVM);
      Task<IActionResult> UpdateAsync(UpdateVM updateVM);
      Task<IActionResult> UpdateRecurrencesAsync(UpdateVM updateVM);
      Task<IActionResult> DeleteAsync(string id);

   }
}
