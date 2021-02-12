using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Transfers
{

   partial class TransferController
   {

      [HttpPost("insert")]
      public Task<IActionResult> InsertAsync([FromBody] InsertVM insertVM) =>
         _TransferService.InsertAsync(insertVM);

   }

}
