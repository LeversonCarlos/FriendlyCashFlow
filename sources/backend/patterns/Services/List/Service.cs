using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Patterns
{
   partial class PatternService
   {

      public async Task<ActionResult<IPatternEntity[]>> ListAsync()
      {

         // LOAD PATTERNS
         var patternsList = await _PatternRepository.ListPatternsAsync();

         // RESULT
         return Shared.Results.Ok(patternsList);
      }

   }
}
