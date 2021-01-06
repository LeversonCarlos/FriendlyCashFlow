namespace Elesse.Accounts
{
   public class InsertVM
   {
      public string Text { get; set; }
      public enAccountType Type { get; set; }
      public short? ClosingDay { get; set; }
      public short? DueDay { get; set; }
   }
}
