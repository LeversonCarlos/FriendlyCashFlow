using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Transfers
{

   partial class TransferController
   {

      [HttpGet("load/{id}")]
      public Task<ActionResult<ITransferEntity>> LoadAsync(string id) =>
         _TransferService.LoadAsync(id);

   }

}
