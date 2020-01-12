using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Transfers
{

   partial class TransfersService
   {

      public async Task<ActionResult<TransferVM>> UpdateAsync(string transferID, TransferVM viewModel)
      {
         try
         {
            var entriesService = this.GetService<Entries.EntriesService>();

            // VALIDATE
            // var validateMessage = await this.ValidateAsync(viewModel);
            // var validateResult = this.GetValue(validateMessage);
            // if (!validateResult) { return validateMessage.Result; }

            // EXPENSE DATA
            var expenseMessage = await entriesService.GetDataAsync(viewModel.ExpenseEntryID);
            var expenseEntry = this.GetValue(expenseMessage);
            if (expenseEntry == null) { return expenseMessage.Result; }
            expenseEntry.AccountID = viewModel.ExpenseAccountID;
            expenseEntry.DueDate = viewModel.TransferDate;
            expenseEntry.PayDate = viewModel.TransferDate;
            expenseEntry.EntryValue = Math.Abs(viewModel.TransferValue);

            // INCOME DATA
            var incomeMessage = await entriesService.GetDataAsync(viewModel.IncomeEntryID);
            var incomeEntry = this.GetValue(incomeMessage);
            if (incomeEntry == null) { return incomeMessage.Result; }
            incomeEntry.AccountID = viewModel.IncomeAccountID;
            incomeEntry.DueDate = viewModel.TransferDate;
            incomeEntry.PayDate = viewModel.TransferDate;
            incomeEntry.EntryValue = Math.Abs(viewModel.TransferValue);

            // APPLY CHANGES
            await entriesService.UpdateAsync(expenseEntry.EntryID, expenseEntry);
            await entriesService.UpdateAsync(incomeEntry.EntryID, incomeEntry);

            // RESULT
            return this.OkResponse(viewModel);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

   partial class TransfersController
   {
      [HttpPut("{id}")]
      [Authorize(Roles = "Editor")]
      public async Task<ActionResult<TransferVM>> UpdateAsync(string id, [FromBody]TransferVM value)
      {
         return await this.GetService<TransfersService>().UpdateAsync(id, value);
      }
   }

}
