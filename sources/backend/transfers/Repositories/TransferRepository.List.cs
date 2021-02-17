using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Transfers
{
   partial class TransferRepository
   {

      public async Task<ITransferEntity[]> ListAsync(int year, int month)
      {
         var limitDate = new DateTime(year, month, 1, 0, 0, 0);

         var list = await _Collection
            .Find(entity => entity.Date >= limitDate)
            .ToListAsync();
         return list.ToArray();

      }

   }
}
