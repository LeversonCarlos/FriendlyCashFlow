using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Balances
{
   partial class BalanceRepository
   {

      public async Task<IBalanceEntity[]> ListAsync(int year, int month)
      {
         var startDate = new DateTime(year, month, 1, 0, 0, 0);
         var finishDate = startDate.AddMonths(1).AddSeconds(-1);
         var list = await _Collection
            .Find(entity => entity.Date >= startDate && entity.Date <= finishDate)
            .ToListAsync();
         return list.ToArray();
      }

   }
}
