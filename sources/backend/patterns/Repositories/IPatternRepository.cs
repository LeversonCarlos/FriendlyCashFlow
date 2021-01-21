using System.Threading.Tasks;

namespace Elesse.Patterns
{
   internal interface IPatternRepository
   {

      Task InsertPatternAsync(IPatternEntity value);
      Task UpdatePatternAsync(IPatternEntity value);
      Task DeletePatternAsync(Shared.EntityID patternID);

      Task<IPatternEntity[]> ListPatternsAsync();
      Task<IPatternEntity> LoadPatternAsync(Shared.EntityID patternID);
      // Task<IPatternEntity[]> SearchPatternsAsync(enCategoryType type, string searchText);

   }
}
