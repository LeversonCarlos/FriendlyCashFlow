using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.Identity
{

   partial class IdentityService
   {

      internal const string WARNING_IDENTITY_INVALID_REGISTER_PARAMETER = "WARNING_IDENTITY_INVALID_REGISTER_PARAMETER";
      public async Task<ActionResult> RegisterAsync(RegisterVM registerVM)
      {

         if (registerVM == null)
            throw new ArgumentException(WARNING_IDENTITY_INVALID_REGISTER_PARAMETER);

         var user = new User(registerVM.UserName, registerVM.Password);

         await _UserCollection.InsertOneAsync(user);

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
