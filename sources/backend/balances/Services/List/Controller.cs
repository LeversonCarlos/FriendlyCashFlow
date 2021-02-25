using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Balances
{

   partial class BalanceController
   {

      [HttpGet("list/{year}/{month}")]
      public Task<ActionResult<IBalanceEntity[]>> ListAsync(int year, int month) =>
         _BalanceService.ListAsync(year, month);

   }

}
