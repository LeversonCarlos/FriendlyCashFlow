using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{

   partial class IdentityService
   {

      public Task<IActionResult> UserAuthAsync(UserAuthVM param)
      {
         using (var interactor = new UserAuthInteractor(_MongoDatabase, _Settings))
         {
            return interactor.ExecuteAsync(param);
         }
      }

   }

   partial interface IIdentityService
   {
      Task<IActionResult> UserAuthAsync(UserAuthVM param);
   }

}
