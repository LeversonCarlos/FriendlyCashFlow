using System.Threading.Tasks;
using Elesse.Shared;

namespace Elesse.Recurrences
{
   partial class RecurrenceService
   {

      public Task UpdateAsync(IRecurrenceEntity recurrence) =>
         throw new System.NotImplementedException();

      /*
      public async Task<EntityID> InsertAsync(IRecurrenceProperties recurrenceProperties)
      {

         var recurrence = RecurrenceEntity.Create(recurrenceProperties);

         await _RecurrenceRepository.InsertAsync(recurrence);

         return recurrence.RecurrenceID;
      }
      */

   }
}
