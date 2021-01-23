namespace Elesse.Entries
{
   partial class EntryService
   {

      internal static EntryService Create() =>
         new EntryService(
            Tests.EntryRepositoryMocker.Create().Build(),
            Patterns.PatternService.Create(),
            Shared.Tests.InsightsServiceMocker.Create().Build()
            );

      internal static EntryService Create(IEntryRepository entryRepository) =>
         new EntryService(
            entryRepository,
            Patterns.PatternService.Create(),
            Shared.Tests.InsightsServiceMocker.Create().Build()
         );

   }
}
