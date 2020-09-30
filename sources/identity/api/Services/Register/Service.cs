using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{

   partial class IdentityService
   {

      public Task<IActionResult> RegisterAsync(RegisterVM registerVM)
      {
         using (var interactor = new RegisterInteractor(_MongoDatabase, _Settings))
         {
            return interactor.ExecuteAsync(registerVM);
         }
      }

   }

   partial interface IIdentityService
   {
      Task<IActionResult> RegisterAsync(RegisterVM registerVM);
   }

}
