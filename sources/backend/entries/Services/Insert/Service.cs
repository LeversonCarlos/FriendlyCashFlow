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

            // INCREASE PATTERN
            Patterns.IPatternEntity pattern = null;
            try { pattern = await _PatternService.IncreaseAsync(insertVM.Pattern); }
            catch (Exception valEx) { return Warning(valEx.Message); }

            // CREATE INSTANCE
            EntryEntity entry = null;
            try
            { entry = EntryEntity.Create(pattern, insertVM.AccountID, insertVM.DueDate, insertVM.EntryValue); }
            catch (Exception valEx) { return Warning(valEx.Message); }

            // SAVE ENTRY
            await _EntryRepository.InsertAsync(entry);

            // TRACK EVENT
            _InsightsService.TrackEvent("Entry Service Insert");

            // RESULT
            return Ok();
         }
         catch (Exception ex) { return Shared.Results.Exception(ex); }
      }

   }

   partial interface IEntryService
   {
      Task<IActionResult> InsertAsync(InsertVM insertVM);
   }

   partial struct WARNINGS
   {
      internal const string INVALID_INSERT_PARAMETER = "INVALID_INSERT_PARAMETER";
   }

}
