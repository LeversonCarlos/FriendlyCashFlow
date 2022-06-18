namespace Lewio.CashFlow.Accounts;

partial class SearchCommand
{

   private async Task<bool> ValidateData()
   {
      try
      {
         await Task.CompletedTask;
         return true;
      }
      catch (Exception ex) { return SetErrorAndReturn(ex); }
   }

}
