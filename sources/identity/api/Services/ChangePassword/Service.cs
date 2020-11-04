using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Elesse.Identity
{

   partial class IdentityService
   {

      public async Task<IActionResult> ChangePasswordAsync(System.Security.Principal.IIdentity identity, ChangePasswordVM changePasswordVM)
      {

         // CURRENT USER 
         if (identity == null || !identity.IsAuthenticated)
            return new BadRequestObjectResult(new string[] { WARNINGS.INVALID_CHANGEPASSWORD_PARAMETER });
         var userID = identity.Name;

         // VALIDATE PARAMETERS
         if (changePasswordVM == null)
            return new BadRequestObjectResult(new string[] { WARNINGS.INVALID_CHANGEPASSWORD_PARAMETER });

         // VALIDATE OLD PASSWORD
         var validateOldPassword = await ValidatePasswordAsync(changePasswordVM.OldPassword);
         if (validateOldPassword.Length > 0)
            return new BadRequestObjectResult(validateOldPassword);

         // VALIDATE NEW PASSWORD
         var validateNewPassword = await ValidatePasswordAsync(changePasswordVM.NewPassword);
         if (validateNewPassword.Length > 0)
            return new BadRequestObjectResult(validateNewPassword);

         // VALIDATE PASSWORDS BETWEEN EACH OTHER
         if (changePasswordVM.OldPassword == changePasswordVM.NewPassword)
            return new BadRequestObjectResult(new string[] { WARNINGS.INVALID_CHANGEPASSWORD_PARAMETER });

         // LOCATE USER
         var user = await _UserRepository.GetUserByUserIDAsync(userID);
         if (user == null)
            return new BadRequestObjectResult(new string[] { WARNINGS.AUTHENTICATION_HAS_FAILED });

         // CHECK CURRENT PASSWORD
         if (changePasswordVM.OldPassword.GetHashedText(_Settings.PasswordSalt) != user.Password)
            return new BadRequestObjectResult(new string[] { WARNINGS.AUTHENTICATION_HAS_FAILED });

         // APPLY NEW PASSWORD
         /*
          * TODO
         user.Password = changePasswordVM.NewPassword.GetHashedText(_Settings.PasswordSalt);
         await _Collection.UpdateOneAsync($"{{'UserName':'{ userName}'}}", user);
         */

         // RESULT
         return new OkResult();
      }

   }

   partial interface IIdentityService
   {
      Task<IActionResult> ChangePasswordAsync(System.Security.Principal.IIdentity identity, ChangePasswordVM changePasswordVM);
   }

   partial struct WARNINGS
   {
      internal const string INVALID_CHANGEPASSWORD_PARAMETER = "WARNING_IDENTITY_INVALID_CHANGEPASSWORD_PARAMETER";
   }

}
