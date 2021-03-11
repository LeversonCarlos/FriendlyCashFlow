namespace Elesse.Entries
{
   internal partial class EntryService : Shared.BaseService, IEntryService
   {

      public EntryService(
         IEntryRepository entryRepository,
         Patterns.IPatternService patternService,
         Balances.IBalanceService balanceService,
         Recurrences.IRecurrenceService recurrenceService,
         Shared.IInsightsService insightsService)
         : base("entries", insightsService)
      {
         _EntryRepository = entryRepository;
         _PatternService = patternService;
         _BalanceService = balanceService;
         _RecurrenceService = recurrenceService;
      }

      readonly IEntryRepository _EntryRepository;
      readonly Patterns.IPatternService _PatternService;
      readonly Balances.IBalanceService _BalanceService;
      readonly Recurrences.IRecurrenceService _RecurrenceService;

   }
}
