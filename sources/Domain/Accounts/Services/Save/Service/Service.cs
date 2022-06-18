using Lewio.CashFlow.Services;

namespace Lewio.CashFlow.Domain.Accounts.Services;

public partial class SaveService : SharedService<SaveRequestModel, SaveResponseModel>
{

   public SaveService(IServiceProvider serviceProvider) : base(serviceProvider) { }

   protected override async Task OnExecuting()
   {

      var validateDataResult = await ValidateData();
      if (!validateDataResult)
         return;

      var saveDataResult = await SaveData();
      if (!saveDataResult)
         return;

      var refreshDataResult = await RefreshData();
      if (!refreshDataResult)
         return;

      SetSuccessAndReturn();
   }

}
