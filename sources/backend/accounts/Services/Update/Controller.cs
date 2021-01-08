using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Accounts
{

   partial class AccountController
   {

      [HttpPut("update")]
      public Task<IActionResult> UpdateAsync([FromBody] UpdateVM updateVM) =>
         _AccountService.UpdateAsync(updateVM);

   }

}
