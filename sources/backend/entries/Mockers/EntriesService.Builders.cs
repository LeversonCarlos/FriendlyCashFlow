namespace Elesse.Entries
{
   partial class EntryService
   {

      internal static EntryService Mock() =>
         new EntryService(
            Tests.EntryRepositoryMocker.Create().Build(),
            Patterns.PatternService.Mock(),
            Shared.Tests.InsightsServiceMocker.Create().Build()
            );

      internal static EntryService Mock(IEntryRepository entryRepository) =>
         new EntryService(
            entryRepository,
            Patterns.PatternService.Mock(),
            Shared.Tests.InsightsServiceMocker.Create().Build()
         );

   }
}
