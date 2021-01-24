namespace Elesse.Entries
{
   partial class EntryService
   {

      internal static EntryService Mock() =>
         Mock(
            Tests.EntryRepositoryMocker.Create().Build(),
            Patterns.Tests.PatternServiceMocker.Create().Build()
         );

      internal static EntryService Mock(IEntryRepository entryRepository) =>
         Mock(
            entryRepository,
            Patterns.PatternService.Builder().Build()
         );

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
