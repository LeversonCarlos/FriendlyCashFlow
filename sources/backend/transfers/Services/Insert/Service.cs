using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

            // INCREASE BALANCE
            await IncreaseBalanceAsync(transfer);

            // TRACK EVENT
            _InsightsService.TrackEvent("Transfer Service Insert");

            // RESULT
            return Ok();
         }
         catch (Exception ex) { return Shared.Results.Exception(ex); }
      }

      private async Task<Balances.IBalanceEntity[]> IncreaseBalanceAsync(TransferEntity transfer)
      {
         var paid = true;
         var date = new DateTime(transfer.Date.Year, transfer.Date.Month, 1, 12, 0, 0);
         var incomeAccountID = transfer.IncomeAccountID;
         var incomeValue = transfer.Value;
         var expenseAccountID = transfer.ExpenseAccountID;
         var expenseValue = transfer.Value * -1;
         var incomeBalance = await _BalanceService.IncreaseAsync(incomeAccountID, date, incomeValue, paid);
         var expenseBalance = await _BalanceService.IncreaseAsync(expenseAccountID, date, expenseValue, paid);
         return new[] { incomeBalance, expenseBalance };
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
