namespace Elesse.Accounts
{

   internal partial class AccountService : IAccountService
   {

      public AccountService(IAccountRepository accountRepository, Shared.IInsightsService insightsService)
      {
         _AccountRepository = accountRepository;
         _InsightsService=insightsService;
      }

      readonly IAccountRepository _AccountRepository;
      readonly Shared.IInsightsService _InsightsService;

   }

   public partial interface IAccountService { }

   internal partial struct WARNINGS { }

}
