using System;

namespace FriendlyCashFlow.API.Transfers
{

   public class TransferVM
   {
      public string TransferID { get; set; }
      public DateTime TransferDate { get; set; }
      public decimal TransferValue { get; set; }

      public long IncomeEntryID { get; set; }
      public long IncomeAccountID { get; set; }
      public Accounts.AccountVM IncomeAccountRow { get; set; }
      // public string IncomeAccountText { get; set; }

      public long ExpenseEntryID { get; set; }
      public long ExpenseAccountID { get; set; }
      public Accounts.AccountVM ExpenseAccountRow { get; set; }
      // public string ExpenseAccountText { get; set; }
   }

}
