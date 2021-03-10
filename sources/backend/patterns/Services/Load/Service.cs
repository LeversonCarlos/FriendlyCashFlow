using System;
using System.Threading.Tasks;

namespace Elesse.Patterns
{
   partial class PatternService
   {

      public async Task<IPatternEntity> LoadAsync(IPatternEntity param)
      {

         // VALIDATE PARAMETERS
         if (param == null)
            throw new ArgumentException(WARNINGS.INVALID_LOAD_PARAMETER);
         if (param.CategoryID == null)
            throw new ArgumentException(WARNINGS.INVALID_CATEGORYID);
         if (string.IsNullOrWhiteSpace(param.Text))
            throw new ArgumentException(WARNINGS.INVALID_TEXT);

         // LOAD PATTERN
         var pattern = await _PatternRepository.LoadAsync(param.Type, param.CategoryID, param.Text);

         // RESULT
         return pattern;
      }

   }
}
