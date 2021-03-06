using System;
using System.Threading.Tasks;

namespace Elesse.Patterns
{

   partial class PatternService
   {

      public async Task<IPatternEntity> DecreaseAsync(IPatternEntity patternVM)
      {

         // VALIDATE PARAMETERS
         if (patternVM == null)
            throw new ArgumentException(WARNINGS.INVALID_DECREASE_PARAMETER);

         // LOAD PATTERN
         var pattern = (PatternEntity)(await _PatternRepository.LoadPatternAsync(patternVM.Type, patternVM.CategoryID, patternVM.Text));

         // IF HADNT FOUND, DO NOTHING
         if (pattern == null)
            return null;

         // DECREASE PATTERN COUNT
         pattern.RowsCount--;
         pattern.RowsDate = DateTime.UtcNow;

         // REMOVE PATTERN OF COUNT REACHS ZERO
         if (pattern.RowsCount <= 0)
            await _PatternRepository.DeleteAsync(pattern.PatternID);

         // SAVE CHANGED PATTERN
         if (pattern.RowsCount > 0)
            await _PatternRepository.UpdateAsync(pattern);

         // RESULT
         return pattern;
      }

   }

   partial struct WARNINGS
   {
      internal const string INVALID_DECREASE_PARAMETER = "INVALID_DECREASE_PARAMETER";
   }

}
