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

   }

}
