namespace Elesse.Accounts
{
   public interface IAccountEntity
   {
      Shared.EntityID AccountID { get; }
      string Text { get; }
      short Type { get; }
      short? ClosingDay { get; }
      short? DueDay { get; }
      bool Active { get; }
   }
}
