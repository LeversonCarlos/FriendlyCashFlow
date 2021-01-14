namespace Elesse.Accounts
{
   partial class AccountService
   {

      internal static AccountService Create() =>
         new AccountService(null);

      internal static AccountService Create(IAccountRepository accountRepository) =>
         new AccountService(accountRepository);

   }
}
