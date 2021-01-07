using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Elesse.Accounts
{

   partial class AccountController
   {

      [HttpPut("update")]
      public Task<IActionResult> UpdateAsync([FromBody] UpdateVM updateVM) =>
         _AccountService.UpdateAsync(updateVM);

   }

}
