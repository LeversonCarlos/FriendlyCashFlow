using System;
using System.Threading.Tasks;

namespace Elesse.Patterns
{

   partial class PatternService
   {

      public async Task<IPatternEntity> AddAsync(IPatternEntity param)
      {

         // VALIDATE PARAMETERS
         if (param == null || param.CategoryID == null || string.IsNullOrWhiteSpace(param.Text))
            return null;

         // LOAD PATTERN
         var pattern = ((PatternEntity)await _PatternRepository.LoadPatternAsync(param.Type, param.CategoryID, param.Text));

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
            pattern = new PatternEntity(param.Type, param.CategoryID, param.Text);
            await _PatternRepository.InsertAsync(pattern);
         }

         // RESULT
         return pattern;
      }

   }

   partial interface IPatternService
   {
      Task<IPatternEntity> AddAsync(IPatternEntity param);
   }

}
