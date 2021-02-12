using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Elesse.Entries
{

   partial class EntryService
   {

      public async Task<ActionResult<IEntryEntity[]>> ListAsync(int year, int month)
      {

         // LOAD ENTRIES
         var entriesList = await _EntryRepository.ListAsync(year, month);

         // RESULT
         return Ok(entriesList);
      }

   }

   partial interface IEntryService
   {
      Task<ActionResult<IEntryEntity[]>> ListAsync(int year, int month);
   }

}
