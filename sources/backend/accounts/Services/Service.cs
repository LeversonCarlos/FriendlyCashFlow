namespace Elesse.Accounts
{
   internal partial class AccountService : Shared.BaseService, IAccountService
   {

      public AccountService(IAccountRepository accountRepository, Shared.IInsightsService insightsService)
         : base("accounts", insightsService)
      {
         _AccountRepository = accountRepository;
      }

      readonly IAccountRepository _AccountRepository;

   }
}
