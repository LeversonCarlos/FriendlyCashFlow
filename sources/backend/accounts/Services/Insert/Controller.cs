using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Elesse.Accounts
{

   partial class AccountController
   {

      [HttpPost("insert")]
      public Task<IActionResult> InsertAsync([FromBody] InsertVM insertVM) =>
         _AccountService.InsertAsync(insertVM);

   }

}
