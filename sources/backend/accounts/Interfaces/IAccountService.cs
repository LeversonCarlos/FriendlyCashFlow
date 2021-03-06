using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Accounts
{
   public partial interface IAccountService
   {

      Task<ActionResult<IAccountEntity[]>> SearchAsync(string searchText);

      Task<ActionResult<IAccountEntity[]>> ListAsync();
      Task<ActionResult<IAccountEntity>> LoadAsync(string id);

      Task<IActionResult> InsertAsync(InsertVM insertVM);
      Task<IActionResult> UpdateAsync(UpdateVM updateVM);
      Task<IActionResult> DeleteAsync(string id);

      Task<IActionResult> ChangeStateAsync(ChangeStateVM changeStateVM);

   }
}
