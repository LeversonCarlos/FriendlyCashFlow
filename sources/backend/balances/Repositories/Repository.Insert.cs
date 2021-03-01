using System.Threading.Tasks;

namespace Elesse.Balances
{
   partial class BalanceRepository
   {

      public Task InsertAsync(IBalanceEntity value) =>
         _Collection
            .InsertOneAsync(value as BalanceEntity);

   }
}
