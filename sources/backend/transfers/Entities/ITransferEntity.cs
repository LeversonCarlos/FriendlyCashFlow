using System;

namespace Elesse.Transfers
{
   public interface ITransferEntity
   {
      Shared.EntityID TransferID { get; }

      Shared.EntityID ExpenseAccountID { get; }
      Shared.EntityID IncomeAccountID { get; }

      DateTime Date { get; }
      decimal Value { get; }

   }
}
