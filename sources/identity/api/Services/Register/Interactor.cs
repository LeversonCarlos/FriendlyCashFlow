using FriendlyCashFlow.Shared;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity.Interactors
{
   internal class Register : Interactor<IUser, IdentitySettings, RegisterVM, IActionResult>
   {

      public Register(IMongoDatabase mongoDatabase, IdentitySettings settings) :
         base(mongoDatabase, settings, IdentityService.CollectionName)
      { }

      internal const string WARNING_IDENTITY_INVALID_REGISTER_PARAMETER = "WARNING_IDENTITY_INVALID_REGISTER_PARAMETER";
      internal const string WARNING_IDENTITY_USERNAME_ALREADY_USED = "WARNING_IDENTITY_USERNAME_ALREADY_USED";

      public override async Task<IActionResult> ExecuteAsync(RegisterVM registerVM)
      {

         // VALIDATE PARAMETERS
         if (registerVM == null)
            return new BadRequestObjectResult(new string[] { WARNING_IDENTITY_INVALID_REGISTER_PARAMETER });

         // VALIDATE USERNAME
         using (var interactor = new Interactors.ValidateUsername(MongoDatabase, Settings))
         {
            var validateUsername = await interactor.ExecuteAsync(registerVM.UserName);
            if (validateUsername.Length > 0)
               return new BadRequestObjectResult(validateUsername);
         }

         // VALIDATE PASSWORD
         using (var interactor = new Interactors.ValidatePassword(MongoDatabase, Settings))
         {
            var validatePassword = await interactor.ExecuteAsync(registerVM.Password);
            if (validatePassword.Length > 0)
               return new BadRequestObjectResult(validatePassword);
         }

         // VALIDATE DUPLICITY
         var usersFound = await Collection.CountDocumentsAsync($"{{'UserName':'{ registerVM.UserName}'}}");
         if (usersFound > 0)
            return new BadRequestObjectResult(new string[] { WARNING_IDENTITY_USERNAME_ALREADY_USED });

         // ADD NEW USER
         var user = new User(registerVM.UserName, registerVM.Password.GetHashedText(Settings.PasswordSalt));
         await Collection.InsertOneAsync(user);

         // SEND ACTIVATION MAIL
         // TODO

         // RESULT
         return new OkResult();
      }
   }
}
