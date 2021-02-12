using System;

namespace Elesse.Transfers
{

   partial class TransferEntity
   {

      public static TransferEntity Create(
         Shared.EntityID expenseAccountID, Shared.EntityID incomeAccountID,
         DateTime date, decimal value) =>
         new TransferEntity
         {
            TransferID = Shared.EntityID.NewID(),
            ExpenseAccountID = expenseAccountID,
            IncomeAccountID = incomeAccountID,
            Date = date,
            Value = value
         };

   }

}
