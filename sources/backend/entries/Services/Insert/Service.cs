using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Elesse.Entries
{
   partial class EntryService
   {

      public async Task<IActionResult> InsertAsync(InsertVM insertVM)
      {
         try
         {

            // VALIDATE PARAMETERS
            if (insertVM == null)
               return Warning(WARNINGS.INVALID_INSERT_PARAMETER);

            // DEFINE RECURRENCE
            /*
            if (insertVM.Recurrence != null)
               insertVM.Recurrence = EntryRecurrenceEntity.Create(insertVM.Recurrence,insertVM.Recurrence);
            */

            // INCREASE PATTERN
            Patterns.IPatternEntity pattern = null;
            try { pattern = await _PatternService.IncreaseAsync(insertVM.Pattern); }
            catch (Exception valEx) { return Warning(valEx.Message); }

            // CREATE INSTANCE
            EntryEntity entry = null;
            try { entry = EntryEntity.Create(pattern, insertVM.AccountID, insertVM.DueDate, insertVM.Value); }
            catch (Exception valEx) { return Warning(valEx.Message); }

            // APPLY PAYMENT
            if (insertVM.Paid && insertVM.PayDate.HasValue)
               entry.SetPayment(insertVM.PayDate.Value, insertVM.Value);

            // APPLY RECURRENCE
            /*
            if (insertVM.Recurrence?.RecurrenceID != null)
               entry.SetRecurrence(insertVM.Recurrence);
            */

            // REFRESH SORTING
            entry.RefreshSorting();

            // SAVE ENTRY
            await _EntryRepository.InsertAsync(entry);

            // INCREASE BALANCE
            await IncreaseBalanceAsync(entry);

            // TRACK EVENT
            _InsightsService.TrackEvent("Entry Service Insert");

            // RESULT
            return Ok();
         }
         catch (Exception ex) { return Shared.Results.Exception(ex); }
      }

      private Task<Balances.IBalanceEntity> IncreaseBalanceAsync(EntryEntity entry)
      {
         var accountID = entry.AccountID;
         var date = new DateTime(entry.SearchDate.Year, entry.SearchDate.Month, 1, 12, 0, 0);
         var value = entry.Value * (entry.Pattern.Type == Patterns.enPatternType.Income ? 1 : -1);
         var paid = entry.Paid;
         return _BalanceService.IncreaseAsync(accountID, date, value, paid);
      }

   }
}
