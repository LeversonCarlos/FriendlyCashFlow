namespace Elesse.Recurrences.Tests
{
   public partial class RecurrenceServiceTests
   {

      Shared.Results Warning(params string[] messageList) =>
         Shared.Results.GetResults("recurrence", Shared.enResultType.Warning, messageList);

   }
}
