using System;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{
   partial class IdentityService
   {

      internal const string WARNING_IDENTITY_INVALID_USERNAME = "WARNING_IDENTITY_INVALID_USERNAME";

      internal async Task<string[]> ValidateUsernameAsync(string username)
      {
         try
         {
            var MailAddress = new System.Net.Mail.MailAddress(username);
            await Task.CompletedTask;

            return new string[] { };
         }
         catch (Exception) { return new string[] { WARNING_IDENTITY_INVALID_USERNAME }; }
      }

   }
}
