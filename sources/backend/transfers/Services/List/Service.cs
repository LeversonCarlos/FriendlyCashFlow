using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Elesse.Transfers
{
   partial class TransferService
   {

      public async Task<ActionResult<ITransferEntity[]>> ListAsync(int year, int month)
      {

         var transfersList = await _TransferRepository.ListAsync(year, month);

         return Ok(transfersList);
      }

   }
}
