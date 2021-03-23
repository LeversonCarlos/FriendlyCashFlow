using System.Threading.Tasks;

namespace Elesse.Recurrences
{
   partial class RecurrenceService
   {

      public async Task UpdateAsync(IRecurrenceEntity recurrenceParam)
      {

         var recurrence = (RecurrenceEntity)(await _RecurrenceRepository.LoadAsync(recurrenceParam.RecurrenceID));

         recurrence.SetProperties(recurrenceParam.Properties);

         await _RecurrenceRepository.UpdateAsync(recurrence);

      }

   }
}
