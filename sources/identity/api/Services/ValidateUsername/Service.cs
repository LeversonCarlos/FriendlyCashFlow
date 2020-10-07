using System;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{
   partial class IdentityService
   {

      internal async Task<string[]> ValidateUsernameAsync(string username)
      {
         try
         {
            var mailAddress = new System.Net.Mail.MailAddress(username);
            await Task.CompletedTask;

            return new string[] { };
         }
         catch (Exception) { return new string[] { WARNINGS.INVALID_USERNAME }; }
      }

   }

   partial struct WARNINGS
   {
      internal const string INVALID_USERNAME = "WARNING_IDENTITY_INVALID_USERNAME";
   }

}
