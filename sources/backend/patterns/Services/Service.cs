namespace Elesse.Patterns
{

   internal partial class PatternService : IPatternService
   {

      public PatternService(IPatternRepository patternRepository, Shared.IInsightsService insightsService)
      {
         _PatternRepository = patternRepository;
         _InsightsService = insightsService;
      }

      readonly IPatternRepository _PatternRepository;
      readonly Shared.IInsightsService _InsightsService;

      Microsoft.AspNetCore.Mvc.BadRequestObjectResult Warning(params string[] messageList) =>
         Shared.Results.Warning("patterns", messageList);

   }

   public partial interface IPatternService { }

   internal partial struct WARNINGS { }

}
