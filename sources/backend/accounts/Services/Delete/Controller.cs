using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Accounts
{

   partial class AccountController
   {

      [HttpDelete("delete/{id}")]
      public Task<IActionResult> DeleteAsync(string id) =>
         _AccountService.DeleteAsync(id);

   }

}
