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
         internal const string INVALID_USERAUTH_PARAMETER = "WARNING_IDENTITY_INVALID_USERAUTH_PARAMETER";
         internal const string AUTHENTICATION_HAS_FAILED = "WARNING_IDENTITY_AUTHENTICATION_HAS_FAILED";
      }

      public override async Task<IActionResult> ExecuteAsync(UserAuthVM param)
      {

         // VALIDATE PARAMETERS
         if (param == null)
            return new BadRequestObjectResult(new string[] { WARNING.INVALID_USERAUTH_PARAMETER });

         // VALIDATE USERNAME
         using (var interactor = new ValidateUsernameInteractor(MongoDatabase, Settings))
         {
            var validateUsername = await interactor.ExecuteAsync(param.UserName);
            if (validateUsername.Length > 0)
               return new BadRequestObjectResult(validateUsername);
         }

         // VALIDATE PASSWORD
         using (var interactor = new ValidatePasswordInteractor(MongoDatabase, Settings))
         {
            var validatePassword = await interactor.ExecuteAsync(param.Password);
            if (validatePassword.Length > 0)
               return new BadRequestObjectResult(validatePassword);
         }

         // LOCATE USER
         /*
         var userCursor = await Collection.FindAsync($"{{'UserName':'{ param.UserName}'}}");
         if (userCursor == null)
            return new BadRequestObjectResult(new string[] { WARNING.AUTHENTICATION_HAS_FAILED });
         var user = await userCursor.FirstOrDefaultAsync();
         if (user == null)
            return new BadRequestObjectResult(new string[] { WARNING.AUTHENTICATION_HAS_FAILED });
         if (param.Password.GetHashedText(Settings.PasswordSalt) != user.Password)
            return new BadRequestObjectResult(new string[] { WARNING.AUTHENTICATION_HAS_FAILED });
         */

         // VALIDATE USER
         // TODO

         // GENEATE IDENTITY
         // TODO

         // CREATE ACCESS TOKEN
         // TODO

         // CREATE REFRESH TOKEN
         // TODO

         // RESULT
         return new OkResult();
      }

   }
}
