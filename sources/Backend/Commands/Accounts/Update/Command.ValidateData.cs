namespace Lewio.CashFlow.Accounts;

partial class UpdateCommand
{

   private async Task<bool> ValidateData()
   {
      try
      {

         _Request.Account.EnsureValidModel();

         await ValidateData_EnsureValidTextProperty();

         return true;
      }
      catch (Exception ex) { return SetErrorAndReturn(ex); }
   }

   private async Task ValidateData_EnsureValidTextProperty()
   {

      var dataList = await _ServiceProvider
         .GetRequiredService<IAccountRepository>()
         .GetListBySearchTerms(_Request.Account.Text);

      if (dataList == null)
         throw new Exception("Error validating the account on database");

      dataList = dataList
         .Where(x => x.AccountID != _Request.Account.AccountID && x.Text == _Request.Account.Text)
         .ToArray();

      if (dataList.Any())
         throw new Exception("An account with this Text property already exists");

   }

}
