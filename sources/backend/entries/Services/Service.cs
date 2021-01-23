namespace Elesse.Entries
{

   internal partial class EntryService : Shared.BaseService, IEntryService
   {

      public EntryService(IEntryRepository entryRepository, Shared.IInsightsService insightsService)
         : base("entries", insightsService)
      {
         _EntryRepository = entryRepository;
      }

      readonly IEntryRepository _EntryRepository;

   }

   public partial interface IEntryService { }

   internal partial struct WARNINGS { }

}
