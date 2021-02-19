using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Entries
{
   partial class EntryRepository
   {

      public async Task<IEntryEntity[]> ListAsync(int year, int month)
      {
         var startDate = new DateTime(year, month, 1, 0, 0, 0);
         var finishDate = startDate.AddMonths(1).AddSeconds(-1);

         var list = await _Collection
            .Find(entity => entity.SearchDate >= startDate && entity.SearchDate <= finishDate)
            .ToListAsync();
         return list.ToArray();

      }

   }
}
