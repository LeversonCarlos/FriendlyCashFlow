using System;

namespace Elesse.Balances
{

   partial class BalanceEntity
   {

      public static BalanceEntity Restore(Shared.EntityID balanceID,
         Shared.EntityID accountID,
         DateTime date, decimal expectedValue, decimal realizedValue) =>
         new BalanceEntity
         {
            BalanceID = balanceID,
            AccountID = accountID,
            Date = date,
            ExpectedValue = expectedValue,
            RealizedValue = realizedValue
         };

   }

}
