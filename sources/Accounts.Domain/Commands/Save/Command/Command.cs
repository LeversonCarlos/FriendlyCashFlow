namespace Lewio.CashFlow.Accounts;

public partial class SaveCommand : AccountsCommand<SaveRequestModel, SaveResponseModel>
{

   public SaveCommand(IServiceProvider serviceProvider) : base(serviceProvider) { }

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
