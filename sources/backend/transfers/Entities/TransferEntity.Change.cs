using System;

namespace Elesse.Transfers
{

   partial class TransferEntity
   {

      public void Change(Shared.EntityID expenseAccountID, Shared.EntityID incomeAccountID,
         DateTime date, decimal value)
      {
         ExpenseAccountID = expenseAccountID;
         IncomeAccountID = incomeAccountID;
         Date = date;
         Value = value;
      }

   }

}
