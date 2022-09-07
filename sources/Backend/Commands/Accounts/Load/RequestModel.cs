namespace Lewio.CashFlow.Accounts;

public partial class LoadRequestModel : RequestModel
{
   public EntityID AccountID { get; set; }
}

partial class LoadRequestModel
{
   public static LoadRequestModel Create(string? accountID)
   {
      var request = new LoadRequestModel();
      if (accountID != null)
         request.AccountID = EntityID.Create(accountID);
      return request;
   }
}
