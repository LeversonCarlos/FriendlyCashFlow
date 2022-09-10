namespace Lewio.CashFlow.Accounts;

public partial class UpdateCommand : Command<UpdateRequestModel, UpdateResponseModel>
{

   public UpdateCommand(IServiceProvider serviceProvider) : base(serviceProvider) { }

   protected override async Task OnHandling()
   {

      if (!await ValidateData())
         return;

      if (!await SaveData())
         return;

      if (!await RefreshData())
         return;

      SetSuccessResult();
   }

}
