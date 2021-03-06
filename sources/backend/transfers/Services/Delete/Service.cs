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
            var transfer = (TransferEntity)await _TransferRepository.LoadAsync(transferID);
            if (transfer == null)
               return Warning(WARNINGS.TRANSFER_NOT_FOUND);

            // DECREASE BALANCE
            await DecreaseBalanceAsync(transfer);

            // REMOVE TRANSFER
            await _TransferRepository.DeleteAsync(transferID);

            // TRACK EVENT
            _InsightsService.TrackEvent("Transfer Service Delete");

            // RESULT
            return Ok();
         }
         catch (Exception ex) { return Shared.Results.Exception(ex); }
      }

      private async Task<Balances.IBalanceEntity[]> DecreaseBalanceAsync(TransferEntity transfer)
      {
         var paid = true;
         var date = new DateTime(transfer.Date.Year, transfer.Date.Month, 1, 12, 0, 0);
         var incomeAccountID = transfer.IncomeAccountID;
         var incomeValue = transfer.Value;
         var expenseAccountID = transfer.ExpenseAccountID;
         var expenseValue = transfer.Value * -1;
         var incomeBalance = await _BalanceService.DecreaseAsync(incomeAccountID, date, incomeValue, paid);
         var expenseBalance = await _BalanceService.DecreaseAsync(expenseAccountID, date, expenseValue, paid);
         return new[] { incomeBalance, expenseBalance };
      }

   }
}
