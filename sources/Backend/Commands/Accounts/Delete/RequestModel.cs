namespace Lewio.CashFlow.Accounts;

public class DeleteRequestModel : RequestModel
{
   public EntityID AccountID { get; set; }

   internal static DeleteRequestModel CreateFrom(string accountID) =>
      new DeleteRequestModel
      {
         AccountID = EntityID.Create(accountID)
      };
}
