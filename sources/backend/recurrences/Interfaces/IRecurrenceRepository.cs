using System;
using System.Threading.Tasks;

namespace Elesse.Recurrences
{
   internal interface IRecurrenceRepository
   {

      Task InsertAsync(IRecurrenceEntity value);
      Task UpdateAsync(IRecurrenceEntity value);
      // Task DeleteAsync(Shared.EntityID id);

      Task<IRecurrenceEntity> LoadAsync(Shared.EntityID recurrenceID);

   }
}
