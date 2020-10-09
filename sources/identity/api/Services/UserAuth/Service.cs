using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{

   partial class IdentityService
   {

      public async Task<ActionResult<TokenVM>> UserAuthAsync(UserAuthVM param)
      {
         try
         {

            // VALIDATE PARAMETERS
            if (param == null)
               return new BadRequestObjectResult(new string[] { WARNINGS.INVALID_USERAUTH_PARAMETER });

            // VALIDATE USERNAME
            var validateUsername = await ValidateUsernameAsync(param.UserName);
            if (validateUsername.Length > 0)
               return new BadRequestObjectResult(validateUsername);

            // VALIDATE PASSWORD
            var validatePassword = await ValidatePasswordAsync(param.Password);
            if (validatePassword.Length > 0)
               return new BadRequestObjectResult(validatePassword);

            // LOCATE USER
            var userCursor = await _Collection.FindAsync($"{{'UserName':'{ param.UserName}'}}");
            if (userCursor == null)
               return new BadRequestObjectResult(new string[] { WARNINGS.AUTHENTICATION_HAS_FAILED });
            var user = await userCursor.FirstOrDefaultAsync();
            if (user == null)
               return new BadRequestObjectResult(new string[] { WARNINGS.AUTHENTICATION_HAS_FAILED });
            if (param.Password.GetHashedText(_Settings.PasswordSalt) != user.Password)
               return new BadRequestObjectResult(new string[] { WARNINGS.AUTHENTICATION_HAS_FAILED });

            // VALIDATE USER
            // TODO

            // CREATE TOKEN
            var result = await CreateAccessTokenAsync(user);

            // RESULT
            return new OkObjectResult(result);
         }
         catch (Exception ex) { return new BadRequestObjectResult(new string[] { ex.Message }); }
      }

   }

   partial interface IIdentityService
   {
      Task<ActionResult<TokenVM>> UserAuthAsync(UserAuthVM param);
   }

   partial struct WARNINGS
   {
      internal const string INVALID_USERAUTH_PARAMETER = "WARNING_IDENTITY_INVALID_USERAUTH_PARAMETER";
      internal const string AUTHENTICATION_HAS_FAILED = "WARNING_IDENTITY_AUTHENTICATION_HAS_FAILED";
      internal const string TOKEN_CREATION_HAS_FAILED = "WARNING_IDENTITY_TOKEN_CREATION_HAS_FAILED";
   }

}
