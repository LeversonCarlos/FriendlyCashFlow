using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Elesse.Accounts
{

   partial class AccountController
   {

      [HttpGet("load/{id}")]
      public Task<ActionResult<IAccountEntity>> LoadAsync(Shared.EntityID id) =>
         _AccountService.LoadAsync(id);

   }

}
