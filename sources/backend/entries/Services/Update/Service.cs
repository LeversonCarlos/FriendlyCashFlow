using System;
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

   }

   partial interface IEntryService
   {
      Task<IActionResult> UpdateAsync(UpdateVM updateVM);
   }

   partial struct WARNINGS
   {
      internal const string INVALID_UPDATE_PARAMETER = "INVALID_UPDATE_PARAMETER";
   }

}
