namespace Elesse.Accounts
{

   internal partial class AccountService : IAccountService
   {

      public AccountService(IAccountRepository accountRepository, Shared.IInsightsService insightsService)
      {
         _AccountRepository = accountRepository;
         _InsightsService = insightsService;
      }

      readonly IAccountRepository _AccountRepository;
      readonly Shared.IInsightsService _InsightsService;

      Microsoft.AspNetCore.Mvc.BadRequestObjectResult Warning(params string[] messageList) =>
         Shared.Results.Warning("accounts", messageList);

   }

   public partial interface IAccountService { }

   internal partial struct WARNINGS { }

}
