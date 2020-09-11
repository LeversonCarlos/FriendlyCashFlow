using System;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{

   partial class IdentityService
   {
      public Task<object> RegisterAsync(object value) => 
         throw new ArgumentException("WARNING_IDENTITY_INVALID_REGISTER_PARAMETER");
   }

   partial interface IIdentityService
   {

      Task<object> RegisterAsync(object value);

   }

}
