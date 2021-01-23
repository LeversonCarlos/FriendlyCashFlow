using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Entries
{

   partial class EntryService
   {

      public async Task<ActionResult<IEntryEntity>> LoadAsync(string id)
      {

         // VALIDATE PARAMETERS
         if (!Shared.EntityID.TryParse(id, out var entryID))
            return Warning(WARNINGS.INVALID_LOAD_PARAMETER);

         // LOAD ENTRY
         var entry = await _EntryRepository.LoadAsync(entryID);

         // RESULT
         return Ok(entry);
      }

   }

   partial interface IEntryService
   {
      Task<ActionResult<IEntryEntity>> LoadAsync(string id);
   }

   partial struct WARNINGS
   {
      internal const string INVALID_LOAD_PARAMETER = "INVALID_ENTRYID_PARAMETER";
   }

}
