namespace Lewio.CashFlow.Domain.Accounts;

partial class IAccountExtension
{

   public static T CreateNew<T>() where T : IAccount =>
      Activator.CreateInstance<T>();

}
