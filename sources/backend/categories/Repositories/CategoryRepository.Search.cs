using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Categories
{
   partial class CategoryRepository
   {

      public async Task<ICategoryEntity[]> SearchCategoriesAsync(enCategoryType type, string searchText)
      {
         var list = await _Collection
            // .Find(filter)
            .Find(entity =>
               entity.RowStatus == true &&
               entity.Type == type &&
               entity.Text.ToLower().Contains(searchText.ToLower())
            )
            .ToListAsync();
         return list.ToArray();
      }

      public async Task<ICategoryEntity[]> SearchCategoriesAsync(enCategoryType type, Shared.EntityID parentID, string searchText)
      {
         var list = await _Collection
            // .Find(filter)
            .Find(entity =>
               entity.RowStatus == true &&
               entity.Type == type &&
               entity.ParentID == parentID &&
               entity.Text.ToLower().Contains(searchText.ToLower())
            )
            .ToListAsync();
         return list.ToArray();
      }

   }
}
