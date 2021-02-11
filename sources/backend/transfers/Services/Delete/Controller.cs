using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Transfers
{

   partial class TransferController
   {

      [HttpDelete("delete/{id}")]
      public Task<IActionResult> DeleteAsync(string id) =>
         _TransferService.DeleteAsync(id);

   }

}
