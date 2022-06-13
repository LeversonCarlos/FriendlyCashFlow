namespace Lewio.CashFlow.Domain.Accounts;

public static class IAccountExtension
{

   public static T To<T>(this IAccount self)
      where T : IAccount
   {
      var result = Activator.CreateInstance<T>();
      result.ID = self.ID;
      result.Type = self.Type;
      result.Text = self.Text;
      return result;
   }

}
