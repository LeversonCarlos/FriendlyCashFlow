using Lewio.CashFlow.Repository;
using Lewio.CashFlow.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Lewio.CashFlow.Domains.Accounts.Services;

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

      _Response.Data = await _ServiceProvider.GetService<IMainRepository>()!.Accounts.SaveNew(account);

      await Task.CompletedTask;
      SetSuccessAndReturn();
   }

}
