namespace Lewio.CashFlow.Accounts;

public partial class SearchCommand : Command<SearchRequestModel, SearchResponseModel>
{

   public SearchCommand(IServiceProvider serviceProvider) : base(serviceProvider) { }

}
