using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Entries
{
   partial class EntryService
   {

      public async Task<IActionResult> UpdateAsync(UpdateVM updateVM)
      {
         try
         {

            // VALIDATE PARAMETERS
            if (updateVM == null)
               return Warning(WARNINGS.INVALID_UPDATE_PARAMETER);

            // LOCATE ENTRY
            var entry = (EntryEntity)await _EntryRepository.LoadAsync(updateVM.EntryID);
            if (entry == null)
               return Warning(WARNINGS.ENTRY_NOT_FOUND);

            // DECREASE BALANCE
            await DecreaseBalanceAsync(entry);

            // CHECK PATTERN CHANGES
            Patterns.IPatternEntity pattern = null;
            try { pattern = await UpdateAsync_GetNewPattern(entry.Pattern, updateVM.Pattern); }
            catch (Exception valEx) { return Warning(valEx.Message); }

            // APPLY COMMOM CHANGES
            try { entry.Change(pattern, updateVM.AccountID, updateVM.DueDate, updateVM.Value); }
            catch (Exception valEx) { return Warning(valEx.Message); }

            // APPLY PAYMENT
            if (updateVM.Paid && updateVM.PayDate.HasValue)
               entry.SetPayment(updateVM.PayDate.Value, updateVM.Value);
            else
               entry.ClearPayment();

            // REFRESH SORTING
            entry.RefreshSorting();

            // SAVE ENTRY
            await _EntryRepository.UpdateAsync(entry);

            // INCREASE BALANCE
            await IncreaseBalanceAsync(entry);

            // TRACK EVENT
            _InsightsService.TrackEvent("Entry Service Update");

            // RESULT
            return Ok();
         }
         catch (Exception ex) { return Shared.Results.Exception(ex); }
      }

      async Task<Patterns.IPatternEntity> UpdateAsync_GetNewPattern(Patterns.IPatternEntity oldPattern, Patterns.IPatternEntity newPattern)
      {

         // PATTERN HASNT CHANGED
         if (newPattern as Patterns.PatternEntity == oldPattern as Patterns.PatternEntity)
            return oldPattern;

         // DECREASE PATTERN
         await _PatternService.DecreaseAsync(oldPattern);

         // INCREASE PATTERN
         Patterns.IPatternEntity pattern = null;
         pattern = await _PatternService.IncreaseAsync(newPattern);

         return pattern;
      }

      public async Task<IActionResult> UpdateRecurrencesAsync(UpdateVM updateVM)
      {
         try
         {

            var updateResult = await UpdateAsync(updateVM);
            if (!(updateResult is OkResult))
               return updateResult;

            if (updateVM.Recurrence == null || updateVM.Recurrence.RecurrenceID == null)
               return new OkResult();

            var recurrences = (EntryEntity[])await _EntryRepository.LoadRecurrencesAsync(updateVM.Recurrence.RecurrenceID);

            var recurrenceVMs = recurrences
               .Where(r => r.Recurrence != null && r.Recurrence.CurrentOccurrence > updateVM.Recurrence.CurrentOccurrence)
               .OrderBy(r => r.Recurrence.CurrentOccurrence)
               .Select(r => new UpdateVM
               {
                  EntryID = r.EntryID,
                  Pattern = updateVM.Pattern,
                  AccountID = r.AccountID,
                  DueDate = r.DueDate,
                  Value = r.Value,
                  Paid = r.Paid,
                  PayDate = r.PayDate
               })
               .ToArray();

            foreach (var recurrenceVM in recurrenceVMs)
            {

               recurrenceVM.Value = updateVM.Value;
               recurrenceVM.DueDate = updateVM.DueDate = updateVM.DueDate.AddMonths(1);

               if (recurrenceVM.Paid)
                  continue;

               var recurrenceResult = await UpdateAsync(recurrenceVM);
               if (!(updateResult is OkResult))
                  return updateResult;

            }

            return new OkResult();
         }
         catch (Exception ex) { return Shared.Results.Exception(ex); }
      }

   }
}
