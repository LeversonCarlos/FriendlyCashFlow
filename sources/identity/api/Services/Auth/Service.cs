using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{

   partial class IdentityService
   {

      public Task<IActionResult> AuthUserAsync(AuthUserVM authUserVM)
      {
         return null;
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
