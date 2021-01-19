using System.Threading.Tasks;
using Elesse.Shared;

namespace Elesse.Categories
{
   partial class CategoryRepository
   {

      public Task<ICategoryEntity[]> SearchCategoriesAsync(enCategoryType type, string searchText) =>
         throw new System.NotImplementedException();

      public Task<ICategoryEntity[]> SearchCategoriesAsync(enCategoryType type, EntityID parentID, string searchText) =>
         throw new System.NotImplementedException();

   }
}
