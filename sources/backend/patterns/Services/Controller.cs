using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Patterns
{

   [Route("api/patterns")]
   [Authorize]
   public partial class PatternController : Controller
   {

      internal readonly IPatternService _PatternService;

      public PatternController(IPatternService patternService)
      {
         _PatternService = patternService;
      }

   }

}
