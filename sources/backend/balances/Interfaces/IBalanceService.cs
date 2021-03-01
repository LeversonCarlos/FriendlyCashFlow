using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Balances
{
   public partial interface IBalanceService
   {
      Task<ActionResult<IBalanceEntity[]>> ListAsync(int year, int month);
      Task<IBalanceEntity> IncreaseAsync(Shared.EntityID accountID, DateTime date, decimal value, bool paid);
      Task<IBalanceEntity> DecreaseAsync(Shared.EntityID accountID, DateTime date, decimal value, bool paid);
   }
}
