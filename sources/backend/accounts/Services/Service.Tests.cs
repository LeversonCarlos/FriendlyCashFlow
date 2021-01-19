namespace Elesse.Accounts.Tests
{
   public partial class AccountServiceTests
   {

      Shared.Results Warning(params string[] messageList) =>
         Shared.Results.GetResults("accounts", Shared.enResultType.Warning, messageList);

   }
}
