using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Balances
{
   partial class BalanceRepository
   {

      public Task UpdateAsync(IBalanceEntity value) =>
         _Collection
            .ReplaceOneAsync(entity => entity.BalanceID == value.BalanceID, value as BalanceEntity);

   }
}
