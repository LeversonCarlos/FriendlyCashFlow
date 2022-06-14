namespace Lewio.CashFlow.Domain.Accounts;

public static class IAccountExtension
{

   public static T To<T>(this IAccount self)
      where T : IAccount
   {
      var result = Activator.CreateInstance<T>();
      result.AccountID = self.AccountID;

      result.Type = self.Type;
      result.Text = self.Text;

      result.CreditCardClosingDay = self.CreditCardClosingDay;
      result.CreditCardDueDay = self.CreditCardDueDay;

      result.IsActive = self.IsActive;
      return result;
   }

}
