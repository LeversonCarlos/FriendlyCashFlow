using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Entries
{

   partial class EntryService
   {

      public async Task<IActionResult> DeleteAsync(string id)
      {

         // VALIDATE PARAMETERS
         if (!Shared.EntityID.TryParse(id, out var entryID))
            return Warning(WARNINGS.INVALID_DELETE_PARAMETER);

         // LOCATE ENTRY
         var entry = await _EntryRepository.LoadAsync(entryID);
         if (entry == null)
            return Warning(WARNINGS.ENTRY_NOT_FOUND);

         // DECREASE PATTERN
         try { await _PatternService.DecreaseAsync(entry.Pattern); }
         catch (Exception valEx) { return Warning(valEx.Message); }

         // REMOVE ENTRY
         await _EntryRepository.DeleteAsync(entryID);

         // TRACK EVENT
         _InsightsService.TrackEvent("Entry Service Delete");

         // RESULT
         return Ok();
      }

   }

   partial interface IEntryService
   {
      Task<IActionResult> DeleteAsync(string id);
   }

   partial struct WARNINGS
   {
      internal const string INVALID_DELETE_PARAMETER = "INVALID_DELETE_PARAMETER";
      internal const string ENTRY_NOT_FOUND = "ENTRY_NOT_FOUND";
   }

}
