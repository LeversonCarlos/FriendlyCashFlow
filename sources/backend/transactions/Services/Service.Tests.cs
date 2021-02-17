namespace Elesse.Transactions.Tests
{
   public partial class TransactionServiceTests
   {

      Shared.Results Warning(params string[] messageList) =>
         Shared.Results.GetResults("transaction", Shared.enResultType.Warning, messageList);

   }
}
