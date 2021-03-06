namespace Elesse.Entries
{
   internal partial class EntryService : Shared.BaseService, IEntryService
   {

      public EntryService(
         IEntryRepository entryRepository,
         Patterns.IPatternService patternService,
         Balances.IBalanceService balanceService,
         Shared.IInsightsService insightsService)
         : base("entries", insightsService)
      {
         _EntryRepository = entryRepository;
         _PatternService = patternService;
         _BalanceService = balanceService;
      }

      readonly IEntryRepository _EntryRepository;
      readonly Patterns.IPatternService _PatternService;
      readonly Balances.IBalanceService _BalanceService;

   }
}
