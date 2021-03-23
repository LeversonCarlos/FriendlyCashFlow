using System;
using System.Threading.Tasks;
using Elesse.Shared;

namespace Elesse.Recurrences
{
   partial class RecurrenceService
   {

      public async Task<EntityID> InsertAsync(IRecurrenceProperties param)
      {

         if (param == null)
            throw new ArgumentException(WARNINGS.INVALID_INSERT_PARAMETER);

         var recurrence = RecurrenceEntity.Create(param);

         await _RecurrenceRepository.InsertAsync(recurrence);

         return recurrence.RecurrenceID;
      }

   }
}
