namespace Elesse.Patterns
{

   internal partial class PatternService : Shared.SharedService, IPatternService
   {

      public PatternService(IPatternRepository patternRepository, Shared.IInsightsService insightsService)
         : base("patterns", insightsService)
      {
         _PatternRepository = patternRepository;
      }

      readonly IPatternRepository _PatternRepository;

   }

   public partial interface IPatternService { }

   internal partial struct WARNINGS { }

}
