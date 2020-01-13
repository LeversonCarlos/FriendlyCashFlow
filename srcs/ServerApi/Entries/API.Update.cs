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

      internal async Task<ActionResult<EntryVM>> UpdateAsync(long entryID, bool editFutureRecurrencies, EntryVM viewModel)
      {
         try
         {

            // VALIDATE
            var validateMessage = await this.ValidateAsync(viewModel);
            var validateResult = this.GetValue(validateMessage);
            if (!validateResult) { return validateMessage.Result; }

            // LOCATE DATA
            var data = await this.GetDataQuery().Where(x => x.EntryID == entryID).FirstOrDefaultAsync();
            if (data == null) { return this.NotFoundResponse(); }

            // REMOVE BALANCE
            await this.GetService<Balances.BalancesService>().RemoveBalanceAsync(data);

            // REMOVE PATTERN
            var newPatternID = await this.GetService<Patterns.PatternsService>().GetPatternIDAsync(viewModel);
            if (data.PatternID.HasValue && data.PatternID.Value != newPatternID)
            { await this.GetService<Patterns.PatternsService>().RemovePatternAsync(data.PatternID.Value); }

            // APPLY CHANGES
            data.Text = viewModel.Text;
            data.CategoryID = viewModel.CategoryID;
            data.DueDate = viewModel.DueDate;
            data.EntryValue = Math.Abs(viewModel.EntryValue);
            data.Paid = viewModel.Paid;
            if (viewModel.Paid && viewModel.PayDate.HasValue) { data.PayDate = viewModel.PayDate; } else { data.PayDate = null; }
            if (viewModel.AccountID.HasValue) { data.AccountID = viewModel.AccountID; }

            // SEARCH DATE
            data.SearchDate = data.DueDate;
            if (data.Paid && data.PayDate.HasValue) { data.SearchDate = data.PayDate.Value; }
            this.ApplySorting(data);

            // ADD PATTERN
            if (!data.PatternID.HasValue || data.PatternID.Value != newPatternID)
            { data.PatternID = await this.GetService<Patterns.PatternsService>().AddPatternAsync(viewModel); }

            // SAVE IT
            await this.dbContext.SaveChangesAsync();

            // ADD BALANCE
            await this.GetService<Balances.BalancesService>().AddBalanceAsync(data);

            // EDIT FUTURE RECURRENCIES
            if (data.RecurrencyID.HasValue && data.RecurrencyID.Value > 0)
            {
               if (editFutureRecurrencies)
               { await this.GetService<Recurrencies.RecurrenciesService>().UpdateAsync(data.RecurrencyID.Value, data.EntryID); }
               await this.GetService<Recurrencies.RecurrenciesService>().UpdatePortionsAsync(data.RecurrencyID.Value);
            }

            // RESULT
            var result = EntryVM.Convert(data);
            return this.OkResponse(result);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

   partial class EntriesController
   {
      [HttpPut("{id:long}/{editFutureRecurrencies:bool}")]
      [HttpPut("{id:long}")]
      [Authorize(Roles = "Editor")]
      public async Task<ActionResult<EntryVM>> UpdateAsync(long id, [FromBody]EntryVM value, bool editFutureRecurrencies = false)
      {
         return await this.GetService<EntriesService>().UpdateAsync(id, editFutureRecurrencies, value);
      }
   }

}
