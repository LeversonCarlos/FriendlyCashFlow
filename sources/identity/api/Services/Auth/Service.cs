using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{

   partial class IdentityService
   {

      public async Task<IActionResult> UserAuthAsync(UserAuthVM param)
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
      Task<IActionResult> UserAuthAsync(UserAuthVM param);
   }

}
