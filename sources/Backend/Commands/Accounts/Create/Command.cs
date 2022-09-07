namespace Lewio.CashFlow.Accounts;

public partial class CreateCommand : Command<CreateRequestModel, CreateResponseModel>
{

   public CreateCommand(IServiceProvider serviceProvider) : base(serviceProvider) { }

   protected override async Task OnHandling()
   {

      if (!await ValidateData())
         return;

      if (!await SaveData())
         return;

      SetSuccessResult();
   }

}
