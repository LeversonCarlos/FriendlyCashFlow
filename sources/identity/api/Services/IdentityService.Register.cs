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

         if (registerVM == null)
            throw new ArgumentException(WARNING_IDENTITY_INVALID_REGISTER_PARAMETER);

         var passwordStrength = await this.ValidatePasswordAsync(registerVM.Password);
         if (passwordStrength is BadRequestObjectResult)
            return passwordStrength;

         var user = new User(registerVM.UserName, registerVM.Password);

         var collection = await GetCollectionAsync();
         await collection.InsertOneAsync(user);

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
