using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Patterns
{
   partial class PatternRepository
   {

      public Task DeleteAsync(Shared.EntityID patternID) =>
         _Collection
            .DeleteOneAsync(entity => entity.PatternID == patternID);

   }
}