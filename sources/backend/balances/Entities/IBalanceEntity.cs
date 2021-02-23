using System;

namespace Elesse.Balances
{

   public partial interface IBalanceEntity
   {
      Shared.EntityID BalanceID { get; }

      Shared.EntityID AccountID { get; }
      DateTime Date { get; }

      decimal ExpectedValue { get; }
      decimal RealizedValue { get; }
   }

}
