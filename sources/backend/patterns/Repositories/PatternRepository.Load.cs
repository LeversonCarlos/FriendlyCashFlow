using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Patterns
{
   partial class PatternRepository
   {

      public async Task<IPatternEntity> LoadPatternAsync(Shared.EntityID patternID) =>
         await _Collection
            .Find(entity => entity.PatternID == patternID)
            .SingleOrDefaultAsync();

      public async Task<IPatternEntity> LoadPatternAsync(enPatternType type, Shared.EntityID categoryID, string text) =>
         await _Collection
            .Find(entity => entity.Type == type && entity.CategoryID == categoryID && entity.Text == text)
            .SortByDescending(entity => entity.RowsDate)
            .SingleOrDefaultAsync();

   }
}
