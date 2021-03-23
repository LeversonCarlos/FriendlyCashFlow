using System;
using System.Threading.Tasks;

namespace Elesse.Recurrences
{
   partial class RecurrenceService
   {

      public async Task UpdateAsync(IRecurrenceEntity param)
      {

         if (param == null)
            throw new ArgumentException(WARNINGS.INVALID_UPDATE_PARAMETER);
         if (param.RecurrenceID == null)
            throw new ArgumentException(WARNINGS.INVALID_RECURRENCEID);
         if (param.Properties == null)
            throw new ArgumentException(WARNINGS.INVALID_PROPERTIES);

         var recurrence = (RecurrenceEntity)(await _RecurrenceRepository.LoadAsync(param.RecurrenceID));
         if (recurrence == null)
            throw new NullReferenceException(WARNINGS.RECURRENCE_NOT_FOUND);

         recurrence.SetProperties(param.Properties);

         await _RecurrenceRepository.UpdateAsync(recurrence);

      }

   }
}
