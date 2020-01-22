using System;

namespace FriendlyCashFlow.API.Dashboard
{

   public class BalanceVM
   {
      public long AccountID { get; set; }
      public string Text { get; set; }
      public Accounts.enAccountType Type { get; set; }
      public decimal CurrentBalance { get; set; }
      public decimal IncomeForecast { get; set; }
      public decimal ExpenseForecast { get; set; }
   }

}
