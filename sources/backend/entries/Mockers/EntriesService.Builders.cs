namespace Elesse.Entries
{
   partial class EntryService
   {

      internal static EntryService Create() =>
         new EntryService(
            Tests.EntryRepositoryMocker.Create().Build(),
            Shared.Tests.InsightsServiceMocker.Create().Build()
            );

      internal static EntryService Create(IEntryRepository entryRepository) =>
         new EntryService(
            entryRepository,
            Shared.Tests.InsightsServiceMocker.Create().Build()
         );

   }
}
