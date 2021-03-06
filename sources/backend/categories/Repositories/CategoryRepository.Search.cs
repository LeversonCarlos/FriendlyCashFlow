using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Categories
{
   partial class CategoryRepository
   {

      public async Task<ICategoryEntity[]> SearchAsync(enCategoryType type, string searchText)
      {
         var list = await _Collection
            // .Find(filter)
            .Find(entity =>
               entity.RowStatus == true &&
               entity.Type == type &&
               (string.IsNullOrWhiteSpace(searchText) || entity.Text.ToLower().Contains(searchText.ToLower()))
            )
            .ToListAsync();
         return list.ToArray();
      }

      public async Task<ICategoryEntity[]> SearchAsync(enCategoryType type, Shared.EntityID parentID, string searchText)
      {
         var list = await _Collection
            // .Find(filter)
            .Find(entity =>
               entity.RowStatus == true &&
               entity.Type == type &&
               entity.ParentID == parentID &&
               (string.IsNullOrWhiteSpace(searchText) || entity.Text.ToLower().Contains(searchText.ToLower()))
            )
            .ToListAsync();
         return list.ToArray();
      }

   }
}
