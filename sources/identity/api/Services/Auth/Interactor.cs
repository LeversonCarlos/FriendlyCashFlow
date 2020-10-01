using FriendlyCashFlow.Shared;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{
   internal class UserAuthInteractor : Interactor<IUser, IdentitySettings, UserAuthVM, IActionResult>
   {

      public UserAuthInteractor(IMongoDatabase mongoDatabase, IdentitySettings settings) :
         base(mongoDatabase, settings, IdentityService.CollectionName)
      { }

      internal struct WARNING
      {
         internal const string INVALID_REGISTER_PARAMETER = "WARNING_IDENTITY_INVALID_REGISTER_PARAMETER";
      }

      public override async Task<IActionResult> ExecuteAsync(UserAuthVM param)
      {

         // RESULT
         await Task.CompletedTask;
         return new BadRequestObjectResult(new string[] { });
      }
   }
}
