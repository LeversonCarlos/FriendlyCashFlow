using System.Threading.Tasks;

namespace Elesse.Categories
{
   internal interface ICategoryRepository
   {

      Task InsertAsync(ICategoryEntity value);
      Task UpdateAsync(ICategoryEntity value);
      Task DeleteAsync(Shared.EntityID categoryID);

      Task<ICategoryEntity[]> ListAsync();
      Task<ICategoryEntity> LoadAsync(Shared.EntityID categoryID);
      Task<ICategoryEntity[]> SearchAsync(enCategoryType type, string searchText);
      Task<ICategoryEntity[]> SearchAsync(enCategoryType type, Shared.EntityID parentID, string searchText);

   }
}
