using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Entries
{
   partial class EntryRepository
   {

      public async Task<IEntryEntity[]> ListAsync(int year, int month)
      {
         var limitDate = new DateTime(year, month, 1, 0, 0, 0);

         var list = await _Collection
            .Find(entity => entity.SearchDate >= limitDate)
            .ToListAsync();
         return list.ToArray();
      }

   }
}
