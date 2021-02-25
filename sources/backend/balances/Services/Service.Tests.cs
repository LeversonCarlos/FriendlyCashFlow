namespace Elesse.Balances.Tests
{
   public partial class BalanceServiceTests
   {

      Shared.Results Warning(params string[] messageList) =>
         Shared.Results.GetResults("balance", Shared.enResultType.Warning, messageList);

   }
}
