using Lewio.CashFlow.Services;

namespace Lewio.CashFlow.Domain.Accounts.Services;

public partial class CreateService : SharedService<CreateRequestModel, CreateResponseModel>
{

   public CreateService(IServiceProvider serviceProvider) : base(serviceProvider) { }

   protected override async Task OnExecuting()
   {

      var saveDataResult = await SaveData();
      if (!saveDataResult)
         return;

      var refreshDataResult = await RefreshData();
      if (!refreshDataResult)
         return;

      SetSuccessAndReturn();
   }

}
