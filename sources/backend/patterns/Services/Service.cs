namespace Elesse.Patterns
{
   internal partial class PatternService : Shared.BaseService, IPatternService
   {

      public PatternService(IPatternRepository patternRepository, Shared.IInsightsService insightsService)
         : base("patterns", insightsService)
      {
         _PatternRepository = patternRepository;
      }

      readonly IPatternRepository _PatternRepository;

   }
}
