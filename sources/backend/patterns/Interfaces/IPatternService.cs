using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Patterns
{
   public interface IPatternService
   {

      Task<ActionResult<IPatternEntity[]>> ListAsync();

      Task<IPatternEntity> IncreaseAsync(IPatternEntity param);
      Task<IPatternEntity> DecreaseAsync(IPatternEntity patternVM);

   }
}
