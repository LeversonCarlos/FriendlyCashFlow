namespace Lewio.CashFlow.Accounts;

public partial class SearchCommand : Command<SearchRequestModel, SearchResponseModel>
{

   public SearchCommand(IServiceProvider serviceProvider) : base(serviceProvider) { }

   protected override async Task OnHandling()
   {

      if (!await SearchData())
         return;

      SetSuccessResult();
   }

}
