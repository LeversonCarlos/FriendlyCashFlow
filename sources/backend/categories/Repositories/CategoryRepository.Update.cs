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

         await UpdateChildrenHierarchyText(categoryEntity.CategoryID);
      }

      async Task UpdateChildrenHierarchyText(Shared.EntityID categoryID)
      {
         var children = await _Collection
            .Find(entity => entity.ParentID == categoryID)
            .ToListAsync();
         foreach (var child in children)
         {
            child.HierarchyText = $"{await GetParentText(categoryID)}{child.Text}";
            var builder = Builders<CategoryEntity>.Update.Set(x => x.HierarchyText, child.HierarchyText);
            await _Collection
               .UpdateOneAsync(entity => entity.CategoryID == child.CategoryID, builder);
            await UpdateChildrenHierarchyText(child.CategoryID);
         }
      }

   }
}
