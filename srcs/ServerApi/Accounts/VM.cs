namespace FriendlyCashFlow.API.Accounts
{
   public enum enAccountType : short { General = 0, Bank = 1, CreditCard = 2, Investment = 3, Service = 4 }
   public class AccountVM
   {

      public long AccountID { get; set; }
      public string Text { get; set; }
      public enAccountType Type { get; set; }
      public short? DueDay { get; set; }
      public bool Active { get; set; }

      internal static AccountVM Convert(AccountData value)
      {
         return new AccountVM
         {
            AccountID = value.AccountID,
            Text = value.Text,
            Type = (enAccountType)value.Type,
            DueDay = value.DueDay,
            Active = value.Active
         };
      }

   }

}
