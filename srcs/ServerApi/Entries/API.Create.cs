using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Entries
{

   partial class EntriesService
   {

      internal async Task<ActionResult<EntryVM>> CreateAsync(EntryVM value)
      {
         try
         {

            // VALIDATE
            var validateMessage = await this.ValidateAsync(value);
            var validateResult = this.GetValue(validateMessage);
            if (!validateResult) { return validateMessage.Result; }

            // NEW MODEL
            var user = this.GetService<Helpers.User>();
            var data = new EntryData()
            {
               ResourceID = user.ResourceID,
               Type = (short)value.Type,
               Text = value.Text,
               CategoryID = value.CategoryID,
               DueDate = value.DueDate,
               EntryValue = Math.Abs(value.EntryValue),
               Paid = value.Paid,
               RowStatus = 1
            };
            if (value.Paid && value.PayDate.HasValue) { data.PayDate = value.PayDate; }
            if (value.AccountID.HasValue) { data.AccountID = value.AccountID; }

            // SEARCH DATE
            data.SearchDate = data.DueDate;
            if (data.Paid && data.PayDate.HasValue) { data.SearchDate = data.PayDate.Value; }

            // PATTERN
            data.PatternID = await this.GetService<Patterns.PatternsService>().AddPatternAsync(value);

            // RECURRENCY
            data.RecurrencyID = value.RecurrencyID;
            if (value.Recurrency != null && data.PatternID.HasValue)
            {
               var recurrency = new Recurrencies.RecurrencyVM
               {
                  PatternID = data.PatternID.Value,
                  AccountID = data.AccountID.Value,
                  EntryDate = data.DueDate,
                  EntryValue = data.EntryValue,
                  Type = value.Recurrency.Type,
                  Count = value.Recurrency.Count
               };
               data.RecurrencyID = await this.GetService<Recurrencies.RecurrenciesService>().AddRecurrencyAsync(recurrency);
            }

            // APPLY
            await this.dbContext.Entries.AddAsync(data);
            await this.dbContext.SaveChangesAsync();

            // SORTING
            this.ApplySorting(data);
            await this.dbContext.SaveChangesAsync();

            // BALANCE
            await this.GetService<Balances.BalancesService>().AddBalanceAsync(data);

            // RECURRENCY
            if (value.Recurrency != null && data.RecurrencyID.HasValue && data.RecurrencyID > 0)
            {
               await this.GetService<Recurrencies.RecurrenciesService>().GenerateRecurrencyAsync(data.RecurrencyID.Value);
            }

            // RESULT
            var result = EntryVM.Convert(data);
            return this.CreatedResponse("entries", result.EntryID, result);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

      internal async Task<ActionResult<bool>> CreateTransferAsync(TransferVM value)
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
            var expenseEntry = new EntryVM
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
            var incomeEntry = new EntryVM
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

   partial class EntriesController
   {

      [HttpPost("")]
      [Authorize(Roles = "Editor")]
      public async Task<ActionResult<EntryVM>> CreateAsync([FromBody]EntryVM value)
      {
         return await this.GetService<EntriesService>().CreateAsync(value);
      }

      [HttpPost("transfer")]
      [Authorize(Roles = "Editor")]
      public async Task<ActionResult<bool>> CreateTransferAsync([FromBody]TransferVM value)
      {
         return await this.GetService<EntriesService>().CreateTransferAsync(value);
      }

   }

}
