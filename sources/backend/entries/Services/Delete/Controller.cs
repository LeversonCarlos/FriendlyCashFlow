using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Entries
{

   partial class EntryController
   {

      [HttpDelete("delete/{id}")]
      public Task<IActionResult> DeleteAsync(string id) =>
         _EntryService.DeleteAsync(id);

   }

}
