using System;

namespace FriendlyCashFlow.API.Entries
{

   public class EntryVM
   {
      public long EntryID { get; set; }

      public Categories.enCategoryType Type { get; set; }
      public string Text { get; set; }
      public long PatternID { get; set; }
      public long CategoryID { get; set; }

      public DateTime DueDate { get; set; }
      public decimal EntryValue { get; set; }

      public bool Paid { get; set; }
      public DateTime? PayDate { get; set; }
      public long? AccountID { get; set; }

      public long? RecurrencyID { get; set; }
      public Recurrencies.RecurrencyVM Recurrency { get; set; }

      internal static EntryVM Convert(EntryData value)
      {

         var result = new EntryVM
         {
            EntryID = value.EntryID,
            Type = (Categories.enCategoryType)value.Type,
            Text = value.Text,
            CategoryID = value.CategoryID,
            DueDate = value.DueDate,
            EntryValue = value.EntryValue,
            Paid = value.Paid,
            PayDate = value.PayDate,
            AccountID = value.AccountID
         };

         return result;
      }


   }

}
