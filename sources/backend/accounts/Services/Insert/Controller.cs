using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Accounts
{

   partial class AccountController
   {

      [HttpPost("insert")]
      public Task<IActionResult> InsertAsync([FromBody] InsertVM insertVM) =>
         _AccountService.InsertAsync(insertVM);

   }

}
