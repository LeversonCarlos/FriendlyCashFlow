namespace Lewio.CashFlow.Accounts;

public partial class LoadCommand : AccountsCommand<LoadRequestModel, LoadResponseModel>
{

   public LoadCommand(IServiceProvider serviceProvider) : base(serviceProvider) { }

   protected override async Task OnExecuting()
   {

      var validateDataResult = await ValidateData();
      if (!validateDataResult)
         return;

      var loadDataResult = await LoadData();
      if (!loadDataResult)
         return;

      SetSuccessAndReturn();
   }

}
