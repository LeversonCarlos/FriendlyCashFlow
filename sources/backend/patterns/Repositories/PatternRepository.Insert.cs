using System.Threading.Tasks;

namespace Elesse.Patterns
{
   partial class PatternRepository
   {

      public Task InsertAsync(IPatternEntity value) =>
         _Collection
            .InsertOneAsync(value as PatternEntity);

   }
}
