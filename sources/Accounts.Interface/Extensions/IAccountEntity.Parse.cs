using System.Linq;

namespace Lewio.CashFlow.Accounts;

partial class IAccountExtension
{

   public static T To<T>(this IAccountEntity self) where T : IAccountEntity
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

   public static async Task<T> As<T>(this Task<IAccountEntity> task) where T : IAccountEntity
   {
      var data = await task;
      var result = data.To<T>();
      return result;
   }

   public static async Task<T[]?> As<T>(this Task<IAccountEntity[]> task) where T : IAccountEntity
   {
      var dataList = await task;
      var result = dataList
         ?.Select(data => data.To<T>())
         ?.ToArray();
      return result;
   }

}
