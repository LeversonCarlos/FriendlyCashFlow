using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Patterns
{

   partial class PatternService
   {

      public async Task<ActionResult<Shared.EntityID>> AddAsync(PatternVM patternVM)
      {

         // VALIDATE PARAMETERS
         if (patternVM == null)
            return Warning(WARNINGS.INVALID_ADD_PARAMETER);

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
      Task<ActionResult<Shared.EntityID>> AddAsync(PatternVM patternVM);
   }

   partial struct WARNINGS
   {
      internal const string INVALID_ADD_PARAMETER = "INVALID_ADD_PARAMETER";
   }

}
