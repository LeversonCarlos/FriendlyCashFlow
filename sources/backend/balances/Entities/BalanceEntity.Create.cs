using System;

namespace Elesse.Balances
{

   partial class BalanceEntity
   {

      public static BalanceEntity Create(
         Shared.EntityID accountID,
         DateTime date, decimal expectedValue, decimal realizedValue) =>
         new BalanceEntity
         {
            BalanceID = Shared.EntityID.NewID(),
            AccountID = accountID,
            Date = date,
            ExpectedValue = expectedValue,
            RealizedValue = realizedValue
         };

   }

}
