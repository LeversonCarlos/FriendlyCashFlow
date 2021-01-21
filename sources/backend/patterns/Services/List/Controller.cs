using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Patterns
{

   partial class PatternController
   {

      [HttpGet("list")]
      public Task<ActionResult<IPatternEntity[]>> ListAsync() =>
         _PatternService.ListAsync();

   }

}
