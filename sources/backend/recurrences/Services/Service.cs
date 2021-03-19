using System.Threading.Tasks;
using Elesse.Shared;

namespace Elesse.Recurrences
{

   internal partial class RecurrenceService : Shared.BaseService, IRecurrenceService
   {

      public RecurrenceService(IRecurrenceRepository recurrenceRepository, Shared.IInsightsService insightsService)
         : base("recurrence", insightsService)
      {
         _RecurrenceRepository = recurrenceRepository;
      }

      readonly IRecurrenceRepository _RecurrenceRepository;

      public Task UpdateAsync(IRecurrenceEntity recurrence) =>
         throw new System.NotImplementedException();

   }

}
