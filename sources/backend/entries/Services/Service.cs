namespace Elesse.Entries
{

   internal partial class EntryService : Shared.BaseService, IEntryService
   {

      public EntryService(IEntryRepository entryRepository, Patterns.IPatternService patternService, Shared.IInsightsService insightsService)
         : base("entries", insightsService)
      {
         _EntryRepository = entryRepository;
         _PatternService = patternService;
      }

      readonly IEntryRepository _EntryRepository;
      readonly Patterns.IPatternService _PatternService;

   }

   public partial interface IEntryService { }

   internal partial struct WARNINGS { }

}
