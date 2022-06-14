using Lewio.CashFlow.Services;

namespace Lewio.CashFlow.Domain.Accounts.Services;

public class CreateService : SharedService<CreateRequestModel, CreateResponseModel>
{

   public CreateService(IServiceProvider serviceProvider) : base(serviceProvider) { }

   protected override async Task OnExecuting()
   {

      var account = new AccountEntity
      {
         AccountID = System.Guid.NewGuid(),
         Type = _Request.Type,
         Text = "New Account",
         IsActive = true
      };

      if (_Request.Type == AccountTypeEnum.CreditCard)
      {
         account.CreditCardClosingDay = 10;
         account.CreditCardDueDay = 15;
      }

      var savedAccount = await _MainRepository.Accounts.SaveNew(account);
      _Response.Data = savedAccount.To<AccountEntity>();

      await Task.CompletedTask;
      SetSuccessAndReturn();
   }

}
