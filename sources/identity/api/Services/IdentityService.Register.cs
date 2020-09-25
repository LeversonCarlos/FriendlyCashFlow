using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{

   partial class IdentityService
   {

      internal const string WARNING_IDENTITY_INVALID_REGISTER_PARAMETER = "WARNING_IDENTITY_INVALID_REGISTER_PARAMETER";
      internal const string WARNING_IDENTITY_INVALID_DATABASE_COLLECTION = "WARNING_IDENTITY_INVALID_DATABASE_COLLECTION";

      public async Task<ActionResult> RegisterAsync(RegisterVM registerVM)
      {

         // VALIDATE PARAMETERS
         if (registerVM == null)
            return new BadRequestObjectResult(new string[] { WARNING_IDENTITY_INVALID_REGISTER_PARAMETER });

         // VALIDATE PASSWORD STRENGTH
         var passwordStrength = await this.ValidatePasswordAsync(registerVM.Password);
         if (passwordStrength is BadRequestObjectResult)
            return passwordStrength;

         // RETRIEVE THE COLLECTION
         var collection = await GetCollectionAsync();

         // VALIDATE DUPLICITY
         // TODO

         // HASH THE PASSWORD
         // TODO

         // ADD NEW USER
         var user = new User(registerVM.UserName, registerVM.Password);
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
