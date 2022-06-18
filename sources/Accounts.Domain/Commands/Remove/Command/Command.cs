namespace Lewio.CashFlow.Accounts;

public partial class RemoveCommand : AccountsCommand<RemoveRequestModel, RemoveResponseModel>
{

   public RemoveCommand(IServiceProvider serviceProvider) : base(serviceProvider) { }

   protected override async Task OnExecuting()
   {

      var validateDataResult = await ValidateData();
      if (!validateDataResult)
         return;

      var removeDataResult = await RemoveData();
      if (!removeDataResult)
         return;

      SetSuccessAndReturn();
   }

}
