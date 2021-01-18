using System.Threading.Tasks;

namespace Elesse.Categories
{
   internal interface ICategoryRepository
   {

      Task InsertCategoryAsync(ICategoryEntity value);
      Task UpdateCategoryAsync(ICategoryEntity value);
      Task DeleteCategoryAsync(Shared.EntityID categoryID);

      Task<ICategoryEntity[]> ListCategoriesAsync();
      Task<ICategoryEntity> LoadCategoryAsync(Shared.EntityID categoryID);
      // Task<ICategoryEntity[]> SearchCategoriesAsync(enCategoryType type, string searchText);

   }
}
