using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Transfers
{

   partial class TransfersService
   {

      private IQueryable<Entries.EntryData> GetDataQuery()
      {
         var user = this.GetService<Helpers.User>();
         return this.dbContext.Entries
            .Where(x => x.RowStatus == (short)Base.enRowStatus.Active && x.ResourceID == user.ResourceID)
            .AsQueryable();
      }

      internal async Task<ActionResult<TransferVM>> GetDataAsync(string transferID)
      {
         try
         {

            // LOAD ENTRIES
            var user = this.GetService<Helpers.User>();
            var entries = await this.GetDataQuery()
               .Where(x => x.TransferID == transferID)
               .ToListAsync();
            if (entries == null || entries.Count != 2) { return this.WarningResponse("TRANSFERS_RECORD_NOT_FOUND_WARNING"); }

            // INIT VIEW MODEL
            var viewModel = new TransferVM
            {
               TransferID = transferID,
               TransferDate = entries[0].DueDate,
               TransferValue = Math.Abs(entries[0].EntryValue)
            };

            // EXPENSE
            var expenseEntry = entries.Where(x => x.Type == (short)Categories.enCategoryType.Expense).FirstOrDefault();
            if (expenseEntry == null) { return this.WarningResponse("TRANSFERS_RECORD_NOT_FOUND_WARNING"); }
            viewModel.ExpenseEntryID = expenseEntry.EntryID;
            if (expenseEntry.AccountID.HasValue)
            {
               var accountMessage = await this.GetService<Accounts.AccountsService>().GetDataAsync(expenseEntry.AccountID.Value);
               viewModel.ExpenseAccountRow = this.GetValue(accountMessage);
               viewModel.ExpenseAccountID = expenseEntry.AccountID.Value;
            }

            // INCOME
            var incomeEntry = entries.Where(x => x.Type == (short)Categories.enCategoryType.Income).FirstOrDefault();
            if (incomeEntry == null) { return this.WarningResponse("TRANSFERS_RECORD_NOT_FOUND_WARNING"); }
            viewModel.IncomeEntryID = incomeEntry.EntryID;
            viewModel.IncomeAccountID = incomeEntry.AccountID.Value;
            if (expenseEntry.AccountID.HasValue)
            {
               var accountMessage = await this.GetService<Accounts.AccountsService>().GetDataAsync(incomeEntry.AccountID.Value);
               viewModel.IncomeAccountRow = this.GetValue(accountMessage);
               viewModel.IncomeAccountID = incomeEntry.AccountID.Value;
            }

            return this.OkResponse(viewModel);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

   partial class TransfersController
   {

      [HttpGet("{id}")]
      public async Task<ActionResult<TransferVM>> GetDataAsync(string id)
      {
         return await this.GetService<TransfersService>().GetDataAsync(id);
      }

   }

}
