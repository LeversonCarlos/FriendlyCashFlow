using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Accounts
{

   partial class AccountController
   {

      [HttpGet("load/{id}")]
      public Task<ActionResult<IAccountEntity>> LoadAsync(string id) =>
         _AccountService.LoadAsync(id);

   }

}
