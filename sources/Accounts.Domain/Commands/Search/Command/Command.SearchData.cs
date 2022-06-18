namespace Lewio.CashFlow.Accounts;

partial class SearchCommand
{

   private async Task<bool> SearchData()
   {
      try
      {

         _Response.DataList = await _AccountRepository
            .Search(_Request.SearchTerms)
            .As<AccountEntity>();

         return true;
      }
      catch (Exception ex) { return SetErrorAndReturn(ex); }
   }

}
