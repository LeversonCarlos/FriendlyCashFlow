using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Entries
{

   [Route("api/entries")]
   [Authorize]
   public partial class EntryController : Shared.BaseController
   {

      internal readonly IEntryService _EntryService;

      public EntryController(IEntryService entryService)
      {
         _EntryService = entryService;
      }

   }

}
