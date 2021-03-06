using System.Threading.Tasks;

namespace Elesse.Patterns
{
   internal interface IPatternRepository
   {

      Task InsertAsync(IPatternEntity value);
      Task UpdateAsync(IPatternEntity value);
      Task DeleteAsync(Shared.EntityID patternID);

      Task<IPatternEntity[]> ListAsync();
      Task<IPatternEntity> LoadAsync(Shared.EntityID patternID);
      Task<IPatternEntity> LoadAsync(enPatternType type, Shared.EntityID categoryID, string text);
      // Task<IPatternEntity[]> SearchPatternsAsync(enCategoryType type, string searchText);

   }
}
