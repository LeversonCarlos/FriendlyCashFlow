using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Recurrences
{
   partial class RecurrenceRepository
   {

      public Task UpdateAsync(IRecurrenceEntity value) =>
         _Collection
            .ReplaceOneAsync(entity => entity.RecurrenceID == value.RecurrenceID, value as RecurrenceEntity);

   }
}
