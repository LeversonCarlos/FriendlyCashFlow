using System.Threading.Tasks;

namespace Elesse.Entries
{
   partial class EntryRepository
   {

      public Task InsertAsync(IEntryEntity value) =>
         _Collection
            .InsertOneAsync(value as EntryEntity);

   }
}
