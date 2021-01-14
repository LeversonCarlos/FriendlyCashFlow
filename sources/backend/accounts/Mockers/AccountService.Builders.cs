namespace Elesse.Accounts
{
   partial class AccountService
   {

      internal static AccountService Create() =>
         new AccountService(
            Tests.AccountRepositoryMocker.Create().Build(),
            Shared.Tests.InsightsServiceMocker.Create().Build()
            );

      internal static AccountService Create(IAccountRepository accountRepository) =>
         new AccountService(
            accountRepository,
            Shared.Tests.InsightsServiceMocker.Create().Build()
         );

      //internal static AccountService Create(IAccountRepository accountRepository, Shared.IInsightsService insightsService) =>
      //   new AccountService(accountRepository, insightsService);

   }
}
