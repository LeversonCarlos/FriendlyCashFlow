using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{

   partial class IdentityService
   {

      public async Task<IActionResult> AuthUserAsync(AuthUserVM authUserVM)
      {
         await Task.CompletedTask;
         return new BadRequestObjectResult(new string[] { });
         /*
         using (var interactor = new RegisterInteractor(_MongoDatabase, _Settings))
         {
            return interactor.ExecuteAsync(registerVM);
         }
         */
      }

   }

   partial interface IIdentityService
   {
      Task<IActionResult> AuthUserAsync(AuthUserVM authUserVM);
   }

}
