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
            var entriesService = this.GetService<Entries.EntriesService>();
            var transferID = Guid.NewGuid().ToString("N");

            // VALIDATE
            // var validateMessage = await this.ValidateAsync(viewModel);
            // var validateResult = this.GetValue(validateMessage);
            // if (!validateResult) { return validateMessage.Result; }

            // ACCOUNTS
            var accountIDs = new long[] { value.ExpenseAccountID, value.IncomeAccountID };
            var accounts = await this.dbContext.Accounts.Where(x => accountIDs.Contains(x.AccountID)).ToListAsync();

            // EXPENSE
            var expenseAccount = accounts.Where(x => x.AccountID == value.ExpenseAccountID).Select(x => x.Text).FirstOrDefault();
            var expenseEntry = new Entries.EntryVM
            {
               TransferID = transferID,
               DueDate = value.TransferDate,
               EntryValue = value.TransferValue,
               Paid = true,
               PayDate = value.TransferDate,
               AccountID = value.ExpenseAccountID,
               Type = Categories.enCategoryType.Expense
            };

            // INCOME
            var incomeAccount = accounts.Where(x => x.AccountID == value.IncomeAccountID).Select(x => x.Text).FirstOrDefault();
            var incomeEntry = new Entries.EntryVM
            {
               TransferID = transferID,
               DueDate = value.TransferDate,
               EntryValue = value.TransferValue,
               Paid = true,
               PayDate = value.TransferDate,
               AccountID = value.IncomeAccountID,
               Type = Categories.enCategoryType.Income
            };

            // DESCRIPTION
            expenseEntry.Text = this.GetTranslation("TRANSFERS_TO_ACCOUNT_DESCRIPTION").Replace("{accountText}", incomeAccount);
            incomeEntry.Text = this.GetTranslation("TRANSFERS_FROM_ACCOUNT_DESCRIPTION").Replace("{accountText}", expenseAccount);

            // APPLY
            await entriesService.CreateAsync(expenseEntry);
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
         if (value == null) { return this.BadRequest(this.ModelState); }
         return await this.GetService<TransfersService>().CreateAsync(value);
      }

   }

}
