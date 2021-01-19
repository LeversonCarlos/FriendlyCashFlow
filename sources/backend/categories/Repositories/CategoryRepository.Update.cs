using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Categories
{
   partial class CategoryRepository
   {

      public Task UpdateCategoryAsync(ICategoryEntity value) =>
         _Collection
            .ReplaceOneAsync(entity => entity.CategoryID == value.CategoryID, value as CategoryEntity);
         // TODO: REVIEW HierarchyText

   }
}
