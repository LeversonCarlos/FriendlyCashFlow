using System;

namespace Elesse.Entries
{

   partial class EntryEntity
   {

      public void ClearPayment()
      {
         Paid = false;
         PayDate = null;
      }

      public void SetPayment(DateTime payDate, decimal value)
      {
         Paid = true;
         PayDate = payDate;
         Value = value;
      }

   }

}
