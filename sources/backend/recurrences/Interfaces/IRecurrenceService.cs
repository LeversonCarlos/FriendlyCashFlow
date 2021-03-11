using System.Threading.Tasks;

namespace Elesse.Recurrences
{
   public partial interface IRecurrenceService
   {
      Task<Shared.EntityID> GetNewRecurrenceAsync(IRecurrenceProperties recurrenceProperties);
      Task UpdateAsync(IRecurrenceEntity recurrence);
   }
}
