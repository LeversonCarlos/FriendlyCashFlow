using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Transfers
{

   partial class TransfersService
   {

      internal async Task<ActionResult<bool>> CreateAsync(TransferVM value)
      {
         try
         {

            var transferID = Guid.NewGuid().ToString();
            var entriesService = this.GetService<Entries.EntriesService>();

            // ACCOUNTS
            var accountIDs = new long[] { value.ExpenseAccountID, value.IncomeAccountID };
            var accounts = await this.dbContext.Accounts.Where(x => accountIDs.Contains(x.AccountID)).ToListAsync();

            // EXPENSE
            var expenseAccount = accounts.Where(x => x.AccountID == value.ExpenseAccountID).Select(x => x.Text).FirstOrDefault();
            var expenseText = this.GetTranslation("ENTRIES_TRANSFER_TO_TEXT").Replace("{accountText}", expenseAccount);
            var expenseEntry = new Entries.EntryVM
            {
               TransferID = transferID,
               Text = expenseText,
               DueDate = value.TransferDate,
               EntryValue = value.TransferValue,
               Paid = true,
               PayDate = value.TransferDate,
               AccountID = value.ExpenseAccountID,
               Type = Categories.enCategoryType.Expense
            };
            await entriesService.CreateAsync(expenseEntry);

            // INCOME
            var incomeAccount = accounts.Where(x => x.AccountID == value.IncomeAccountID).Select(x => x.Text).FirstOrDefault();
            var incomeText = this.GetTranslation("ENTRIES_TRANSFER_FROM_TEXT").Replace("{accountText}", incomeAccount);
            var incomeEntry = new Entries.EntryVM
            {
               TransferID = transferID,
               Text = incomeText,
               DueDate = value.TransferDate,
               EntryValue = value.TransferValue,
               Paid = true,
               PayDate = value.TransferDate,
               AccountID = value.IncomeAccountID,
               Type = Categories.enCategoryType.Income
            };
            await entriesService.CreateAsync(incomeEntry);

            // RESULT
            return this.OkResponse(true);

         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

   partial class TransfersController
   {

      [HttpPost("")]
      [Authorize(Roles = "Editor")]
      public async Task<ActionResult<bool>> CreateAsync([FromBody]TransferVM value)
      {
         return await this.GetService<TransfersService>().CreateAsync(value);
      }

   }

}
