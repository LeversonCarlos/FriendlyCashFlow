using System;

namespace FriendlyCashFlow.API.Recurrencies
{
   public enum enRecurrencyType : short { Fixed = 0, Weekly = 1, Monthly = 2, Bimonthly = 3, Quarterly = 4, SemiYearly = 5, Yearly = 6 }
   public class RecurrencyTypeVM : Base.EnumVM<enRecurrencyType> { }

   public class RecurrencyVM
   {
      public long RecurrencyID { get; set; }
      public long PatternID { get; set; }
      public long AccountID { get; set; }

      public enRecurrencyType Type { get; set; }
      public short Count { get; set; }

      public DateTime EntryDate { get; set; }
      public decimal EntryValue { get; set; }
   }

}
