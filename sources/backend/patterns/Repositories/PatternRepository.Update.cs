using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Patterns
{
   partial class PatternRepository
   {

      public Task UpdateAsync(IPatternEntity value) =>
         _Collection
            .ReplaceOneAsync(entity => entity.CategoryID == value.CategoryID, value as PatternEntity);

   }
}