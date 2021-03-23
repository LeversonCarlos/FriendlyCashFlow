using System.Threading.Tasks;

namespace Elesse.Recurrences
{
   public partial interface IRecurrenceService
   {
      Task<Shared.EntityID> InsertAsync(IRecurrenceProperties recurrenceProperties);
      Task UpdateAsync(IRecurrenceEntity recurrence);
   }
}
