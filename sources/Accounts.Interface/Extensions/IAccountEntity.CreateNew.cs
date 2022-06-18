namespace Lewio.CashFlow.Accounts;

partial class IAccountExtension
{

   public static T CreateNew<T>() where T : IAccountEntity =>
      Activator.CreateInstance<T>();

}
