namespace Elesse.Accounts
{
   public class UpdateVM
   {
      public Shared.EntityID AccountID { get; set; }
      public string Text { get; set; }
      public enAccountType Type { get; set; }
      public short? ClosingDay { get; set; }
      public short? DueDay { get; set; }
   }
}
