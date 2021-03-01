namespace Elesse.Entries
{
   partial class EntryService
   {
      public static Tests.EntryServiceBuilder Builder() => new Tests.EntryServiceBuilder();

      internal static EntryService Mock(Patterns.IPatternService patternService) =>
         Mock(
            Tests.EntryRepositoryMocker.Create().Build(),
            patternService
         );

      internal static EntryService Mock(IEntryRepository entryRepository, Patterns.IPatternService patternService) =>
         Mock(
            entryRepository,
            patternService,
            Balances.BalanceService.Builder().Build()
         );

      internal static EntryService Mock(IEntryRepository entryRepository, Patterns.IPatternService patternService, Balances.IBalanceService balanceService) =>
         new EntryService(
            entryRepository,
            patternService,
            balanceService,
            Shared.Tests.InsightsServiceMocker.Create().Build()
         );

   }
}
namespace Elesse.Entries.Tests
{
   internal class EntryServiceBuilder
   {

      IEntryRepository _EntryRepository = Tests.EntryRepositoryMocker.Create().Build();
      public EntryServiceBuilder With(IEntryRepository entryRepository)
      {
         _EntryRepository = entryRepository;
         return this;
      }

      Patterns.IPatternService _PatternService = Patterns.Tests.PatternServiceMocker.Create().Build();
      public EntryServiceBuilder With(Patterns.IPatternService patternService)
      {
         _PatternService = patternService;
         return this;
      }

      Balances.IBalanceService _BalanceService = Balances.BalanceService.Mocker().Build();
      public EntryServiceBuilder With(Balances.IBalanceService balanceService)
      {
         _BalanceService = balanceService;
         return this;
      }

      Shared.IInsightsService _InsightsService = Shared.Tests.InsightsServiceMocker.Create().Build();
      public EntryServiceBuilder With(Shared.IInsightsService insightsService)
      {
         _InsightsService = insightsService;
         return this;
      }

      public EntryService Build() =>
         new EntryService(_EntryRepository, _PatternService, _BalanceService, _InsightsService);

   }
}
