using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Entries
{

   partial class EntryController
   {

      [HttpPut("update")]
      public Task<IActionResult> UpdateAsync([FromBody] UpdateVM updateVM) =>
         _EntryService.UpdateAsync(updateVM);

   }

}
