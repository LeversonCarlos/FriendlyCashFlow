using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Entries
{

   partial class EntryController
   {

      [HttpGet("list/{year}/{month}")]
      public Task<ActionResult<IEntryEntity[]>> ListAsync(int year, int month) =>
         _EntryService.ListAsync(year, month);

   }

}
