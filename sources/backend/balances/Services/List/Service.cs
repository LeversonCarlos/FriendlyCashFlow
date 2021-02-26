using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Elesse.Balances
{
   partial class BalanceService
   {

      public async Task<ActionResult<IBalanceEntity[]>> ListAsync(int year, int month)
      {

         var balanceList = await _BalanceRepository.ListAsync(year, month);

         return Ok(balanceList);
      }

   }
}
