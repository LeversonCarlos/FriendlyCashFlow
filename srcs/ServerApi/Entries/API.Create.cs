using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Entries
{

   partial class EntriesService
   {

      public async Task<ActionResult<EntryVM>> CreateAsync(EntryVM value)
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

            // AUXILIARY
            data.PatternID = await this.GetService<Patterns.PatternsService>().AddPatternAsync(value);
            data.RecurrencyID = await this.GetService<Recurrencies.RecurrenciesService>().AddRecurrencyAsync(value.Recurrency);

            // APPLY
            await this.dbContext.Entries.AddAsync(data);
            await this.dbContext.SaveChangesAsync();

            // SORTING
            this.ApplySorting(data);
            await this.dbContext.SaveChangesAsync();

            // BALANCE
            await this.GetService<Balances.BalancesService>().AddBalanceAsync(data);

            // RESULT
            var result = EntryVM.Convert(data);
            return this.CreatedResponse("entries", result.EntryID, result);
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
   }

}
