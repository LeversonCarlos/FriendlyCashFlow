namespace Lewio.CashFlow.Accounts;

partial class IAccountExtension
{

   public static bool IsNew<T>(this T self) where T : IAccountEntity =>
      !self.AccountID.HasValue || self.AccountID.Value == Guid.Empty;

}
