using System.Threading.Tasks;
using Elesse.Shared;

namespace Elesse.Categories
{
   internal class CategoryRepository : ICategoryRepository
   {

      public Task<ICategoryEntity[]> ListCategoriesAsync() =>
         throw new System.NotImplementedException();

      public Task<ICategoryEntity> LoadCategoryAsync(EntityID categoryID) =>
         throw new System.NotImplementedException();

      public Task<ICategoryEntity[]> SearchCategoriesAsync(enCategoryType type, string searchText) =>
         throw new System.NotImplementedException();

      public Task<ICategoryEntity[]> SearchCategoriesAsync(enCategoryType type, EntityID parentID, string searchText) =>
         throw new System.NotImplementedException();

      public Task InsertCategoryAsync(ICategoryEntity value) =>
         throw new System.NotImplementedException();

      public Task UpdateCategoryAsync(ICategoryEntity value) =>
         throw new System.NotImplementedException();

      public Task DeleteCategoryAsync(EntityID categoryID) =>
         throw new System.NotImplementedException();

   }
}
