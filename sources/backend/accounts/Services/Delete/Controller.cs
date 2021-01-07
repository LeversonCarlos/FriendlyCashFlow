using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Elesse.Accounts
{

   partial class AccountController
   {

      [HttpDelete("delete/{id}")]
      public Task<IActionResult> DeleteAsync(Shared.EntityID id) =>
         _AccountService.DeleteAsync(id);

   }

}
