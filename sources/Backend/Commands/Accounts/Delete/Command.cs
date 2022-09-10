namespace Lewio.CashFlow.Accounts;

public partial class DeleteCommand : Command<DeleteRequestModel, DeleteResponseModel>
{

   public DeleteCommand(IServiceProvider serviceProvider) : base(serviceProvider) { }

   protected override async Task OnHandling()
   {

      if (!await ValidateData())
         return;

      if (!await DeleteData())
         return;

      SetSuccessResult();
   }

}
