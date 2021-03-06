using System.Threading.Tasks;

namespace Elesse.Patterns
{
   internal interface IPatternRepository
   {

      Task InsertAsync(IPatternEntity value);
      Task UpdateAsync(IPatternEntity value);
      Task DeleteAsync(Shared.EntityID patternID);

      Task<IPatternEntity[]> ListPatternsAsync();
      Task<IPatternEntity> LoadPatternAsync(Shared.EntityID patternID);
      Task<IPatternEntity> LoadPatternAsync(enPatternType type, Shared.EntityID categoryID, string text);
      // Task<IPatternEntity[]> SearchPatternsAsync(enCategoryType type, string searchText);

   }
}
