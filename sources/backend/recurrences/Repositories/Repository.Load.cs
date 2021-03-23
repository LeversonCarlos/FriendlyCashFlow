using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Recurrences
{
   partial class RecurrenceRepository
   {

      public async Task<IRecurrenceEntity> LoadAsync(Shared.EntityID recurrenceID) =>
         await _Collection
            .Find(entity => entity.RecurrenceID == recurrenceID)
            .SingleOrDefaultAsync();

   }
}
