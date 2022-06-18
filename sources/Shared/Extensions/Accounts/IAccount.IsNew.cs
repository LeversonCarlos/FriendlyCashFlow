namespace Lewio.CashFlow.Domain.Accounts;

partial class IAccountExtension
{

   public static bool IsNew<T>(this T self) where T : IAccount =>
      !self.AccountID.HasValue || self.AccountID.Value == Guid.Empty;

}
