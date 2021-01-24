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
         new EntryService(
            entryRepository,
            patternService,
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

      Shared.IInsightsService _InsightsService = Shared.Tests.InsightsServiceMocker.Create().Build();
      public EntryServiceBuilder With(Shared.IInsightsService insightsService)
      {
         _InsightsService = insightsService;
         return this;
      }

      public EntryService Build() =>
         new EntryService(_EntryRepository, _PatternService, _InsightsService);

   }
}
