using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{

   partial class IdentityService
   {

      internal const string WARNING_IDENTITY_INVALID_REGISTER_PARAMETER = "WARNING_IDENTITY_INVALID_REGISTER_PARAMETER";
      internal const string WARNING_IDENTITY_USERNAME_ALREADY_USED = "WARNING_IDENTITY_USERNAME_ALREADY_USED";

      public async Task<ActionResult> RegisterAsync(RegisterVM registerVM)
      {

         // VALIDATE PARAMETERS
         if (registerVM == null)
            return new BadRequestObjectResult(new string[] { WARNING_IDENTITY_INVALID_REGISTER_PARAMETER });

         // VALIDATE PASSWORD
         var validatePassword = await ValidatePasswordAsync(registerVM.Password);
         if (validatePassword?.Length > 0)
            return new BadRequestObjectResult(validatePassword);

         // RETRIEVE THE COLLECTION
         var collection = await GetCollectionAsync();

         // VALIDATE DUPLICITY
         var usersFound = await collection.CountDocumentsAsync(Builders<IUser>.Filter.Eq(x => x.UserName, registerVM.UserName));
         if (usersFound > 0)
            return new BadRequestObjectResult(new string[] { WARNING_IDENTITY_USERNAME_ALREADY_USED });

         // ADD NEW USER
         var user = new User(registerVM.UserName, registerVM.Password.GetHashedText(_Settings.PasswordSalt));
         await collection.InsertOneAsync(user);

         // SEND ACTIVATION MAIL
         // TODO

         // RESULT
         return new OkResult();
      }

   }

   partial interface IIdentityService
   {
      Task<ActionResult> RegisterAsync(RegisterVM registerVM);
   }

   public class RegisterVM
   {
      public string UserName { get; set; }
      public string Password { get; set; }
   }

}
