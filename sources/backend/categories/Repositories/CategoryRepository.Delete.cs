using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Categories
{
   partial class CategoryRepository
   {

      public Task DeleteAsync(Shared.EntityID categoryID) =>
         _Collection
            .UpdateOneAsync(entity => entity.CategoryID == categoryID, Builders<CategoryEntity>.Update.Set(x => x.RowStatus, false));

   }
}
