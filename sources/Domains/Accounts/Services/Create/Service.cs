using Lewio.CashFlow.Services;

namespace Lewio.CashFlow.Domains.Accounts.Services;

public class CreateService : SharedService<CreateRequestModel, CreateResponseModel>
{

   public CreateService(IServiceProvider serviceProvider) : base(serviceProvider) { }

   protected override async Task OnExecuting()
   {

      _Response.Data = new AccountEntity
      {
         ID = System.Guid.NewGuid(),
         Type = _Request.Type,
         Text = "New Account"
      };

      await Task.CompletedTask;
      SetSuccessAndReturn();
   }

}
