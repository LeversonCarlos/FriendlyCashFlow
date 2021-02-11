using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Transfers
{

   partial class TransferService
   {

      public async Task<IActionResult> DeleteAsync(string id)
      {
         try
         {

            // VALIDATE PARAMETERS
            if (!Shared.EntityID.TryParse(id, out var transferID))
               return Warning(WARNINGS.INVALID_DELETE_PARAMETER);

            // LOCATE TRANSFER
            var transfer = await _TransferRepository.LoadAsync(transferID);
            if (transfer == null)
               return Warning(WARNINGS.TRANSFER_NOT_FOUND);

            // REMOVE TRANSFER
            await _TransferRepository.DeleteAsync(transferID);

            // TRACK EVENT
            _InsightsService.TrackEvent("Transfer Service Delete");

            // RESULT
            return Ok();
         }
         catch (Exception ex) { return Shared.Results.Exception(ex); }
      }

   }

   partial interface ITransferService
   {
      Task<IActionResult> DeleteAsync(string id);
   }

   partial struct WARNINGS
   {
      internal const string INVALID_DELETE_PARAMETER = "INVALID_DELETE_PARAMETER";
   }

}
