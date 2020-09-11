using System;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{

   partial class IdentityService
   {

      internal const string WARNING_IDENTITY_INVALID_REGISTER_PARAMETER = "WARNING_IDENTITY_INVALID_REGISTER_PARAMETER";
      public Task<object> RegisterAsync(RegisterVM registerVM)
      {

         if (registerVM == null)
            throw new ArgumentException(WARNING_IDENTITY_INVALID_REGISTER_PARAMETER);

         var user = new User(registerVM.UserName, registerVM.Password);

      }

   }

   partial interface IIdentityService
   {
      Task<object> RegisterAsync(RegisterVM registerVM);
   }

   public class RegisterVM
   {
      public string UserName { get; set; }
      public string Password { get; set; }
   }

}
