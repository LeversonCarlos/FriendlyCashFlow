using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Entries
{
   partial class EntryRepository
   {

      public Task UpdateAsync(IEntryEntity value) =>
         _Collection
            .ReplaceOneAsync(entity => entity.EntryID == value.EntryID, value as EntryEntity);

   }
}
