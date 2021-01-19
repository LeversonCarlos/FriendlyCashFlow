using System.Threading.Tasks;

namespace Elesse.Categories
{
   partial class CategoryRepository
   {

      public Task InsertCategoryAsync(ICategoryEntity value) =>
         _Collection
            .InsertOneAsync(value as CategoryEntity);
         // TODO: REVIEW HierarchyText

   }
}
