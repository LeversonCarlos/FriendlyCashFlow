using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Categories
{
   partial class CategoryRepository
   {

      public async Task UpdateCategoryAsync(ICategoryEntity value)
      {

         // COMPOSE HierarchyText BASED ON PARENT TEXT
         var categoryEntity = value as CategoryEntity;
         categoryEntity.HierarchyText = $"{await GetParentText(value.ParentID)}{categoryEntity.Text}";

         await _Collection
            .ReplaceOneAsync(entity => entity.CategoryID == categoryEntity.CategoryID, categoryEntity);
      }

   }
}
