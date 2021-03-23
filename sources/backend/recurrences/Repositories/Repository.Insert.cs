using System.Threading.Tasks;

namespace Elesse.Recurrences
{
   partial class RecurrenceRepository
   {

      public Task InsertAsync(IRecurrenceEntity value) =>
         _Collection
            .InsertOneAsync(value as RecurrenceEntity);

   }
}
