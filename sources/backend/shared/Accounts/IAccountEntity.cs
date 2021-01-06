namespace Elesse.Accounts
{
   public enum enAccountType : short { General = 0, Bank = 1, CreditCard = 2, Investment = 3, Service = 4 }
   public interface IAccountEntity
   {
      Shared.EntityID AccountID { get; }
      string Text { get; }
      enAccountType Type { get; }
      short? ClosingDay { get; }
      short? DueDay { get; }
      bool Active { get; }
   }
}
