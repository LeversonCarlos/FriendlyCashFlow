using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
            var data = new EntryData()
            {
               ResourceID = this.GetService<Helpers.User>().ResourceID,
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
            if (!string.IsNullOrEmpty(value.TransferID)) { data.TransferID = value.TransferID; }

            // SEARCH DATE
            data.SearchDate = data.DueDate;
            if (data.Paid && data.PayDate.HasValue) { data.SearchDate = data.PayDate.Value; }

            // PATTERN
            data.PatternID = await this.GetService<Patterns.PatternsService>().AddAsync(value);

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
               data.RecurrencyID = await this.GetService<Recurrencies.RecurrenciesService>().CreateAsync(recurrency);
            }

            // APPLY
            await this.dbContext.Entries.AddAsync(data);
            await this.dbContext.SaveChangesAsync();

            // SORTING
            this.ApplySorting(data);
            await this.dbContext.SaveChangesAsync();

            // BALANCE
            await this.GetService<Balances.BalancesService>().AddAsync(data);

            // RECURRENCY
            if (value.Recurrency != null && data.RecurrencyID.HasValue && data.RecurrencyID > 0)
            {
               await this.GetService<Recurrencies.RecurrenciesService>().AddEntriesAsync(data.RecurrencyID.Value);
               await this.GetService<Recurrencies.RecurrenciesService>().UpdatePortionsAsync(data.RecurrencyID.Value);
            }

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
         if (value == null) { return this.BadRequest(this.ModelState); }
         return await this.GetService<EntriesService>().CreateAsync(value);
      }

   }

}
