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

         // INCREMENT PATTERN COUNT
         if (pattern != null)
         {
            pattern.RowsCount++;
            pattern.RowsDate = DateTime.UtcNow;
            await _PatternRepository.UpdateAsync(pattern);
         }

         // ADD NEW PATTERN
         if (pattern == null)
         {
            pattern = new PatternEntity(patternVM.Type, patternVM.CategoryID, patternVM.Text);
            await _PatternRepository.InsertAsync(pattern);
         }

         // RESULT
         return Ok(pattern.PatternID);
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
