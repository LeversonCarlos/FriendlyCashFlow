namespace Elesse.Transfers.Tests
{
   public partial class TransferServiceTests
   {

      Shared.Results Warning(params string[] messageList) =>
         Shared.Results.GetResults("transfer", Shared.enResultType.Warning, messageList);

   }
}
