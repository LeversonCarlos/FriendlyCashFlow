namespace Lewio.CashFlow.Accounts;

partial class IAccountExtension
{

   public static T Apply<T>(this T self, T value) where T : IAccountEntity
   {

      self.Type = value.Type;
      self.Text = value.Text;

      self.CreditCardClosingDay = value.CreditCardClosingDay;
      self.CreditCardDueDay = value.CreditCardDueDay;

      self.IsActive = value.IsActive;

      return self;
   }

}
