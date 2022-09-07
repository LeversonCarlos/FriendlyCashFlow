namespace Lewio.CashFlow.Accounts;

partial class SearchCommand
{

   private async Task<bool> SearchData()
   {
      try
      {

         var dataList = await _ServiceProvider
            .GetRequiredService<IAccountRepository>()
            .GetListBySearchTerms(_Request.SearchTerms);

         _Response.Accounts = dataList
            ?.Select(data => data.ToModel())
            ?.ToArray();

         return true;
      }
      catch (Exception ex) { return SetErrorAndReturn(ex); }
   }

}
