using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Elesse.Transfers
{

   partial class TransferService
   {

      public async Task<IActionResult> InsertAsync(InsertVM insertVM)
      {
         try
         {

            // VALIDATE PARAMETERS
            if (insertVM == null)
               return Warning(WARNINGS.INVALID_INSERT_PARAMETER);

            // CREATE INSTANCE
            TransferEntity transfer = null;
            try { transfer = TransferEntity.Create(insertVM.ExpenseAccountID, insertVM.IncomeAccountID, insertVM.Date, insertVM.Value); }
            catch (Exception valEx) { return Warning(valEx.Message); }

            // REFRESH SORTING
            transfer.RefreshSorting();

            // SAVE ENTITY
            await _TransferRepository.InsertAsync(transfer);

            // TRACK EVENT
            _InsightsService.TrackEvent("Transfer Service Insert");

            // RESULT
            return Ok();
         }
         catch (Exception ex) { return Shared.Results.Exception(ex); }
      }

   }

   partial interface ITransferService
   {
      Task<IActionResult> InsertAsync(InsertVM insertVM);
   }

   partial struct WARNINGS
   {
      internal const string INVALID_INSERT_PARAMETER = "INVALID_INSERT_PARAMETER";
   }

}
