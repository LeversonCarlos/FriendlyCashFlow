using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Balances
{
   partial class BalanceRepository
   {

      public async Task<IBalanceEntity> LoadAsync(Shared.EntityID accountID, DateTime date) =>
         await _Collection
            .Find(entity => entity.AccountID == accountID && entity.Date == date)
            .SingleOrDefaultAsync();

   }
}
