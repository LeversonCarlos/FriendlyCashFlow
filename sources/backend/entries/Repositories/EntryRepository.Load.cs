using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Entries
{
   partial class EntryRepository
   {

      public async Task<IEntryEntity> LoadAsync(Shared.EntityID entryID) =>
         await _Collection
            .Find(entity => entity.EntryID == entryID)
            .SingleOrDefaultAsync();

      public async Task<IEntryEntity[]> LoadRecurrencesAsync(Shared.EntityID recurrenceID)
      {
         var list = await _Collection
            .Find(entity => entity.Recurrence.RecurrenceID == recurrenceID)
            .ToListAsync();
         return list.ToArray();
      }

   }
}
