namespace Elesse.Accounts
{

   internal partial class AccountService : IAccountService
   {

      readonly IAccountRepository _AccountRepository;

      public AccountService(IAccountRepository accountRepository)
      {
         _AccountRepository = accountRepository;
      }

   }

   public partial interface IAccountService { }

   internal partial struct WARNINGS { }

}
