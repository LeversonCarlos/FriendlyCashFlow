namespace Lewio.CashFlow.Domain.Accounts;

internal static class IAccountExtension
{

   public static AccountEntity ToEntity(this IAccount self) =>
      new AccountEntity
      {
         ID = self.ID,
         Type = self.Type,
         Text = self.Text
      };

}
