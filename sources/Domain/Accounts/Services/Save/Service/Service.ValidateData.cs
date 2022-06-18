namespace Lewio.CashFlow.Accounts;

partial class SaveService
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
