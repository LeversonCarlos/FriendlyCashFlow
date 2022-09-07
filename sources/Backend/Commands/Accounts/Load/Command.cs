namespace Lewio.CashFlow.Accounts;

public partial class LoadCommand : Command<LoadRequestModel, LoadResponseModel>
{

   public LoadCommand(IServiceProvider serviceProvider) : base(serviceProvider) { }

   protected override async Task OnHandling()
   {

      if (!await LoadData())
         return;

      SetSuccessResult();
   }

}
