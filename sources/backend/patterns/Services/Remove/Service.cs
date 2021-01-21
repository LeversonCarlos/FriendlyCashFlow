using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Patterns
{

   partial class PatternService
   {

      public async Task<IActionResult> RemoveAsync(PatternVM patternVM)
      {

         // VALIDATE PARAMETERS
         if (patternVM == null)
            return Warning(WARNINGS.INVALID_REMOVE_PARAMETER);

         // LOAD PATTERN
         var pattern = (PatternEntity)(await _PatternRepository.LoadPatternAsync(patternVM.Type, patternVM.CategoryID, patternVM.Text));

         // IF HADNT FOUND, DO NOTHING
         if (pattern == null)
            return Ok();

         // DECREMENT PATTERN COUNT
         pattern.RowsCount--;
         pattern.RowsDate = DateTime.UtcNow;

         // REMOVE PATTERN OF COUNT REACHS ZERO
         if (pattern.RowsCount <= 0)
            await _PatternRepository.DeleteAsync(pattern.PatternID);

         // SAVE CHANGED PATTERN
         if (pattern.RowsCount > 0)
            await _PatternRepository.UpdateAsync(pattern);

         // RESULT
         return Ok();
      }

   }

   partial interface IPatternService
   {
      Task<IActionResult> RemoveAsync(PatternVM patternVM);
   }

   partial struct WARNINGS
   {
      internal const string INVALID_REMOVE_PARAMETER = "INVALID_REMOVE_PARAMETER";
   }

}
