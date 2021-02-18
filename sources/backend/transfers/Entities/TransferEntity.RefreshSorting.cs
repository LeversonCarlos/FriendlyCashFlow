using System;

namespace Elesse.Transfers
{

   partial class TransferEntity
   {

      public void RefreshSorting()
      {
         Sorting = Convert.ToInt64(Date.Subtract(new DateTime(1901, 1, 1)).TotalDays);

         var longEntityID = TransferID.ToLong();
         Sorting += ((decimal)longEntityID / (decimal)Math.Pow(10, longEntityID.ToString().Length));
      }

   }

}
