using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Transfers
{

   partial class TransferService
   {

      public async Task<ActionResult<ITransferEntity>> LoadAsync(string id)
      {

         if (!Shared.EntityID.TryParse(id, out var transferID))
            return Warning(WARNINGS.INVALID_LOAD_PARAMETER);

         var transfer = await _TransferRepository.LoadAsync(transferID);

         return Ok(transfer);
      }

   }

   partial struct WARNINGS
   {
      internal const string INVALID_LOAD_PARAMETER = "INVALID_LOAD_PARAMETER";
   }

}
