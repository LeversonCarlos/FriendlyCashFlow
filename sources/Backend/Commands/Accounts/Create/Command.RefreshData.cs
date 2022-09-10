namespace Lewio.CashFlow.Accounts;

partial class CreateCommand
{

   private async Task<bool> RefreshData()
   {
      try
      {

         var request = LoadRequestModel.Create(_Response.AccountID);

         var response = await _ServiceProvider
            .GetRequiredService<LoadCommand>()
            .HandleAsync(request);
         response.EnsureValidResponse();

         _Response.Account = response.Account!;

         return true;
      }
      catch (Exception ex) { return SetErrorAndReturn(ex); }
   }

}