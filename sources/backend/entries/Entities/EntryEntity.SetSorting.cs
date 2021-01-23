using System;

namespace Elesse.Entries
{

   partial class EntryEntity
   {

      public void SetSorting()
      {
         SearchDate = DueDate;
         if (Paid && PayDate.HasValue)
            SearchDate = PayDate.Value;

         Sorting = System.Convert.ToInt64(SearchDate.Subtract(new DateTime(1901, 1, 1)).TotalDays);

         var longEntityID = Convert(EntryID);
         Sorting += ((decimal)longEntityID / (decimal)Math.Pow(10, longEntityID.ToString().Length));
      }

      long Convert(Shared.EntityID entityID)
      {

         var bytes = entityID.Value.ToByteArray();
         Array.Resize(ref bytes, 17);

         var bigInt = new System.Numerics.BigInteger(bytes);

         var logValue = System.Numerics.BigInteger.Log10(bigInt);
         var doubleValue = logValue * Math.Pow(10, 15);
         var intValue = Math.Round(doubleValue, 0);

         return (long)intValue;
      }

   }

}
