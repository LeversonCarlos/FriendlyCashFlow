using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Accounts
{
   partial class AccountRepository
   {

      public async Task<IAccountEntity[]> ListAsync()
      {
         var list = await _Collection
            .Find(entity => entity.RowStatus == true)
            .ToListAsync();
         return list.ToArray();
      }

   }
}
