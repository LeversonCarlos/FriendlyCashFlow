using System;

namespace FriendlyCashFlow.API.Accounts
{

   public enum enAccountType : short { General = 0, Bank = 1, CreditCard = 2, Investment = 3, Service = 4 }
   public class AccountTypeVM : Base.EnumVM<enAccountType> { }

   public class AccountVM
   {

      public long AccountID { get; set; }
      public string Text { get; set; }
      public enAccountType Type { get; set; }
      public short? ClosingDay { get; set; }
      public short? DueDay { get; set; }
      public DateTime? DueDate { get; set; }
      public bool Active { get; set; }

      internal static AccountVM Convert(AccountData value)
      {
         return new AccountVM
         {
            AccountID = value.AccountID,
            Text = value.Text,
            Type = (enAccountType)value.Type,
            ClosingDay = value.ClosingDay,
            DueDay = value.DueDay,
            DueDate = GetDueDate(value),
            Active = value.Active
         };
      }

      private static DateTime? GetDueDate(AccountData value)
      {
         if (value.Type != (short)enAccountType.CreditCard) { return null; }
         if (!value.DueDay.HasValue || value.DueDay.Value == 0) { return null; }
         if (!value.ClosingDay.HasValue || value.ClosingDay.Value == 0) { return null; }

         var dueDay = value.DueDay.Value;
         var closingDay = value.ClosingDay.Value;
         var now = DateTime.Now;
         var dueDate = new DateTime(now.Year, now.Month, dueDay, 0, 0, 0);

         if (now.Day < closingDay && dueDate.Day < closingDay) { dueDate = dueDate.AddMonths(1); }
         if (now.Day >= closingDay && dueDate.Day < closingDay) { dueDate = dueDate.AddMonths(1); }
         if (now.Day >= closingDay) { dueDate = dueDate.AddMonths(1); }

         return dueDate;
      }

   }

}
