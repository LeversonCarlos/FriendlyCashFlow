using System;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{

   partial class IdentityService
   {

      internal const string WARNING_IDENTITY_INVALID_REGISTER_PARAMETER = "WARNING_IDENTITY_INVALID_REGISTER_PARAMETER";
      public Task<object> RegisterAsync(RegisterVM value)
      {
         throw new ArgumentException(WARNING_IDENTITY_INVALID_REGISTER_PARAMETER);
      }

   }

   partial interface IIdentityService
   {
      Task<object> RegisterAsync(RegisterVM value);
   }

   public class RegisterVM
   {
      public string UserName { get; set; }
      public string Password { get; set; }
   }

}
