using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Entries
{

   partial class EntryController
   {

      [HttpPost("insert")]
      public Task<IActionResult> InsertAsync([FromBody] InsertVM insertVM) =>
         _EntryService.InsertAsync(insertVM);

   }

}
