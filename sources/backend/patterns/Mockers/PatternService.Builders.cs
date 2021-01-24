namespace Elesse.Patterns
{
   partial class PatternService
   {
      public static Tests.PatternServiceBuilder Builder() => new Tests.PatternServiceBuilder();
   }
}
namespace Elesse.Patterns.Tests
{
   internal class PatternServiceBuilder
   {

      IPatternRepository _PatternRepository = PatternRepositoryMocker.Create().Build();
      public PatternServiceBuilder With(IPatternRepository patternRepository)
      {
         _PatternRepository = patternRepository;
         return this;
      }

      Shared.IInsightsService _InsightsService = Shared.Tests.InsightsServiceMocker.Create().Build();
      public PatternServiceBuilder With(Shared.IInsightsService insightsService)
      {
         _InsightsService = insightsService;
         return this;
      }

      public PatternService Build() =>
         new PatternService(_PatternRepository, _InsightsService);

   }
}
