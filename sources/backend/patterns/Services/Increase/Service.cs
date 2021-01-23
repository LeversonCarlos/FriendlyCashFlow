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
         var pattern = ((PatternEntity)await _PatternRepository.LoadPatternAsync(param.Type, param.CategoryID, param.Text));

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
            pattern = new PatternEntity(param.Type, param.CategoryID, param.Text);
            await _PatternRepository.InsertAsync(pattern);
         }

         // RESULT
         return pattern;
      }

   }

   partial interface IPatternService
   {
      Task<IPatternEntity> IncreaseAsync(IPatternEntity param);
   }

   partial struct WARNINGS
   {
      internal const string INVALID_INCREASE_PARAMETER = "INVALID_INCREASE_PARAMETER";
   }

}
