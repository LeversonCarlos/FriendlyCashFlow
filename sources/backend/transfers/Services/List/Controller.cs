using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Transfers
{

   partial class TransferController
   {

      [HttpGet("list/{year}/{month}")]
      public Task<ActionResult<ITransferEntity[]>> ListAsync(int year, int month) =>
         _TransferService.ListAsync(year, month);

   }

}
