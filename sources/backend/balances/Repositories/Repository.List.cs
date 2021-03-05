using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Balances
{
   partial class BalanceRepository
   {

      public async Task<IBalanceEntity[]> ListAsync(int year, int month)
      {
         var limitDate = new DateTime(year, month, 1, 0, 0, 0);
         limitDate = limitDate.AddSeconds(-1);
         var list = await _Collection
            .Find(entity => entity.Date <= limitDate)
            .ToListAsync();
         return ListAsync_GetMergedArray(year, month, list);
      }

      IBalanceEntity[] ListAsync_GetMergedArray(int year, int month, List<BalanceEntity> balanceList)
      {
         var dict = new Dictionary<Shared.EntityID, BalanceListValues>();

         foreach (var balance in balanceList)
         {
            if (!dict.ContainsKey(balance.AccountID))
               dict.Add(balance.AccountID, new BalanceListValues());
            var item = dict[balance.AccountID];
            item.ExpectedValue += balance.ExpectedValue;
            item.RealizedValue += balance.RealizedValue;
         }

         var date = new DateTime(year, month, 1, 12, 0, 0);
         var mergedList = dict
            .Select(x=> BalanceEntity.Create(x.Key, date, x.Value.ExpectedValue, x.Value.RealizedValue))
            .ToArray();

         return mergedList;
         // return balanceList.ToArray();
      }

      class BalanceListValues
      {
         public decimal ExpectedValue{ get; set; } = 0;
         public decimal RealizedValue{ get; set; } = 0;
      }

   }
}
