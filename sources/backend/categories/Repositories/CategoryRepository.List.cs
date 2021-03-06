using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Categories
{
   partial class CategoryRepository
   {

      public async Task<ICategoryEntity[]> ListAsync()
      {
         var list = await _Collection
            .Find(entity => entity.RowStatus == true)
            .ToListAsync();
         return list.ToArray();
      }

   }
}
