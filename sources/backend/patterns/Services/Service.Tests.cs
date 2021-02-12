namespace Elesse.Patterns.Tests
{
   public partial class PatternServiceTests
   {

      Shared.Results Warning(params string[] messageList) =>
         Shared.Results.GetResults("patterns", Shared.enResultType.Warning, messageList);

   }
}
