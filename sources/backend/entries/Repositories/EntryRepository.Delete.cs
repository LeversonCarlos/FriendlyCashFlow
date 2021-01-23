using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Entries
{
   partial class EntryRepository
   {

      public Task DeleteAsync(Shared.EntityID entryID) =>
         _Collection
            .DeleteOneAsync(entity => entity.EntryID == entryID);

   }
}
