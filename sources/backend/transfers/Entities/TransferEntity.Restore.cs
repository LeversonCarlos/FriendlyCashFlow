using System;

namespace Elesse.Transfers
{

   partial class TransferEntity
   {

      public static TransferEntity Restore(Shared.EntityID transferID,
         Shared.EntityID expenseAccountID, Shared.EntityID incomeAccountID,
         DateTime date, decimal value) =>
         new TransferEntity
         {
            TransferID = transferID,
            ExpenseAccountID = expenseAccountID,
            IncomeAccountID = incomeAccountID,
            Date = date,
            Value = value
         };

   }

}
