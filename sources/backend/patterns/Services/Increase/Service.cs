using System;
using System.Threading.Tasks;

namespace Elesse.Patterns
{
   partial class PatternService
   {

      public async Task<IPatternEntity> IncreaseAsync(IPatternEntity param)
      {

         // VALIDATE PARAMETERS
         if (param == null)
            throw new ArgumentException(WARNINGS.INVALID_INCREASE_PARAMETER);
         if (param.CategoryID == null)
            throw new ArgumentException(WARNINGS.INVALID_CATEGORYID);
         if (string.IsNullOrWhiteSpace(param.Text))
            throw new ArgumentException(WARNINGS.INVALID_TEXT);

         // LOAD PATTERN
         var pattern = ((PatternEntity)await _PatternRepository.LoadAsync(param.Type, param.CategoryID, param.Text));

         // INCREASE PATTERN COUNT
         if (pattern != null)
         {
            pattern.RowsCount++;
            pattern.RowsDate = DateTime.UtcNow;
            await _PatternRepository.UpdateAsync(pattern);
         }

         // ADD NEW PATTERN
         if (pattern == null)
         {
            pattern = PatternEntity.Create(param.Type, param.CategoryID, param.Text);
            pattern.RowsCount++;
            await _PatternRepository.InsertAsync(pattern);
         }

         // RESULT
         return pattern;
      }

   }
}
