using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Transfers
{

   partial class TransferService
   {

      public async Task<IActionResult> UpdateAsync(UpdateVM updateVM)
      {
         try
         {

            // VALIDATE PARAMETERS
            if (updateVM == null)
               return Warning(WARNINGS.INVALID_UPDATE_PARAMETER);

            // LOCATE TRANSFER
            var transfer = (TransferEntity)await _TransferRepository.LoadAsync(updateVM.TransferID);
            if (transfer == null)
               return Warning(WARNINGS.TRANSFER_NOT_FOUND);

            // APPLY CHANGES
            try { transfer.Change(updateVM.ExpenseAccountID, updateVM.IncomeAccountID, updateVM.Date, updateVM.Value); }
            catch (Exception valEx) { return Warning(valEx.Message); }

            // SAVE TRANSFER
            await _TransferRepository.UpdateAsync(transfer);

            // TRACK EVENT
            _InsightsService.TrackEvent("Transfer Service Update");

            // RESULT
            return Ok();
         }
         catch (Exception ex) { return Shared.Results.Exception(ex); }
      }

   }

   partial interface ITransferService
   {
      Task<IActionResult> UpdateAsync(UpdateVM updateVM);
   }

   partial struct WARNINGS
   {
      internal const string INVALID_UPDATE_PARAMETER = "INVALID_UPDATE_PARAMETER";
      internal const string TRANSFER_NOT_FOUND = "TRANSFER_NOT_FOUND";
   }

}
