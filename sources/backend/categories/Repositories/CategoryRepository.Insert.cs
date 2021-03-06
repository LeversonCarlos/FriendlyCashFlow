using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Categories
{
   partial class CategoryRepository
   {

      public async Task InsertAsync(ICategoryEntity value)
      {

         // COMPOSE HierarchyText BASED ON PARENT TEXT
         var categoryEntity = value as CategoryEntity;
         categoryEntity.HierarchyText = $"{await GetParentText(value.ParentID)}{categoryEntity.Text}";

         await _Collection
            .InsertOneAsync(categoryEntity);
      }

      async Task<string> GetParentText(Shared.EntityID parentID)
      {
         if (parentID == null)
            return "";

         var parentText = await _Collection
            .Find(entity => entity.CategoryID == parentID)
            .Project(entity => entity.HierarchyText)
            .SingleOrDefaultAsync();
         if (string.IsNullOrWhiteSpace(parentText))
            return "";

         return $"{parentText} / ";
      }

   }
}
