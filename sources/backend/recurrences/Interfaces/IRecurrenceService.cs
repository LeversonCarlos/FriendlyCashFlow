using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Recurrences
{
   public partial interface IRecurrenceService
   {
      Task<Shared.EntityID> GetNewRecurrenceAsync(IRecurrenceEntityProperties recurrenceProperties);
      Task UpdateAsync(IRecurrenceEntity recurrence);
   }
}
