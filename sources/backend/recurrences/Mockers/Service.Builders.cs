namespace Elesse.Recurrences
{
   partial class RecurrenceService
   {
      public static Tests.RecurrenceServiceBuilder Builder() => new Tests.RecurrenceServiceBuilder();
   }
}
namespace Elesse.Recurrences.Tests
{
   internal class RecurrenceServiceBuilder
   {

      IRecurrenceRepository _RecurrenceRepository = RecurrenceRepository.Mocker().Build();
      public RecurrenceServiceBuilder With(IRecurrenceRepository recurrenceRepository)
      {
         _RecurrenceRepository = recurrenceRepository;
         return this;
      }

      Shared.IInsightsService _InsightsService = Shared.Tests.InsightsServiceMocker.Create().Build();
      public RecurrenceServiceBuilder With(Shared.IInsightsService insightsService)
      {
         _InsightsService = insightsService;
         return this;
      }

      public RecurrenceService Build() =>
         new RecurrenceService(_RecurrenceRepository, _InsightsService);

   }
}
