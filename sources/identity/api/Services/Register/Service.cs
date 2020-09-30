using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{

   partial class IdentityService
   {

      public Task<IActionResult> RegisterAsync(RegisterVM registerVM)
      {
         using (var interactor = new Interactors.Register(_MongoDatabase, _Settings))
         {
            return interactor.ExecuteAsync(registerVM);
         }
      }

   }

   partial interface IIdentityService
   {
      Task<IActionResult> RegisterAsync(RegisterVM registerVM);
   }

   public class RegisterVM
   {
      public string UserName { get; set; }
      public string Password { get; set; }
   }

}
