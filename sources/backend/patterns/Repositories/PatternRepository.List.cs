using System.Threading.Tasks;
using Elesse.Shared;
using MongoDB.Driver;

namespace Elesse.Patterns
{
   partial class PatternRepository
   {

      public Task<IPatternEntity[]> ListPatternsAsync() =>
         throw new System.NotImplementedException();

   }
}
