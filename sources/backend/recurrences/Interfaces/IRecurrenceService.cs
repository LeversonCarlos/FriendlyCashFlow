using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Recurrences
{
   public partial interface IRecurrenceService
   {
      Task<Shared.EntityID> InsertAsync(IRecurrenceEntityProperties recurrenceProperties);
      Task UpdateAsync(IRecurrenceEntity recurrence);
   }
}
