using System;
using Elesse.Shared;

namespace Elesse.Transfers
{
   public class UpdateVM
   {

      public EntityID TransferID { get; set; }

      public Shared.EntityID ExpenseAccountID { get; set; }
      public Shared.EntityID IncomeAccountID { get; set; }

      public DateTime Date { get; set; }
      public decimal Value { get; set; }

   }
}
