namespace Lewio.CashFlow.Accounts;

partial class DeleteCommand
{

   private async Task<bool> ValidateData()
   {
      try
      {

         _Request.AccountID
            .EnsureValid();

         await Task.CompletedTask;
         return true;
      }
      catch (Exception ex) { return SetErrorAndReturn(ex); }
   }

}
