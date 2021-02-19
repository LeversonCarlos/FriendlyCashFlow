using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Transfers
{
   partial class TransferRepository
   {

      public async Task<ITransferEntity[]> ListAsync(int year, int month)
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
