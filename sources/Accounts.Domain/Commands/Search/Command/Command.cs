namespace Lewio.CashFlow.Accounts;

public partial class SearchCommand : AccountsCommand<SearchRequestModel, SearchResponseModel>
{

   public SearchCommand(IServiceProvider serviceProvider) : base(serviceProvider) { }

   protected override async Task OnExecuting()
   {

      var validateDataResult = await ValidateData();
      if (!validateDataResult)
         return;

      var searchDataResult = await SearchData();
      if (!searchDataResult)
         return;

      SetSuccessAndReturn();
   }

}
