namespace Elesse.Entries.Tests
{
   public partial class EntryServiceTests
   {

      Shared.Results Warning(params string[] messageList) =>
         Shared.Results.GetResults("entries", Shared.enResultType.Warning, messageList);

   }
}
