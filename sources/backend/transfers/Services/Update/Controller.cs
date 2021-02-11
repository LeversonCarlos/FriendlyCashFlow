using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Transfers
{

   partial class TransferController
   {

      [HttpPut("update")]
      public Task<IActionResult> UpdateAsync([FromBody] UpdateVM updateVM) =>
         _TransferService.UpdateAsync(updateVM);

   }

}
