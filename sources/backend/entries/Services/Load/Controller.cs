using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Entries
{

   partial class EntryController
   {

      [HttpGet("load/{id}")]
      public Task<ActionResult<IEntryEntity>> LoadAsync(string id) =>
         _EntryService.LoadAsync(id);

   }

}
