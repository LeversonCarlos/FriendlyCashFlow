namespace Lewio.CashFlow.Accounts;

public partial class SaveService : AccountsService<SaveRequestModel, SaveResponseModel>
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
