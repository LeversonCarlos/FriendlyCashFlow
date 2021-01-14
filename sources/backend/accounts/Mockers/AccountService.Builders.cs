namespace Elesse.Accounts
{
   partial class AccountService
   {

      internal static AccountService Create() =>
         new AccountService(Tests.AccountRepositoryMocker.Create().Build());

      internal static AccountService Create(IAccountRepository accountRepository) =>
         new AccountService(accountRepository);

   }
}
