using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Categories
{
   partial class CategoryRepository
   {

      public async Task<ICategoryEntity> LoadCategoryAsync(Shared.EntityID categoryID) =>
         await _Collection
            .Find(entity => entity.CategoryID == categoryID)
            .SingleOrDefaultAsync();


   }
}
