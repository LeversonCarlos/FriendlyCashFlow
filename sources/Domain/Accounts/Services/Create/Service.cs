using Lewio.CashFlow.Services;

namespace Lewio.CashFlow.Domain.Accounts.Services;

public class CreateService : SharedService<CreateRequestModel, CreateResponseModel>
{

   public CreateService(IServiceProvider serviceProvider) : base(serviceProvider) { }

   protected override async Task OnExecuting()
   {

      var account = new AccountEntity
      {
         ID = System.Guid.NewGuid(),
         Type = _Request.Type,
         Text = "New Account"
      };

      var savedAccount = await _MainRepository.Accounts.SaveNew(account);

      await Task.CompletedTask;
      SetSuccessAndReturn();
   }

}
