using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Elesse.Identity
{

   partial class IdentityService
   {

      public async Task<IActionResult> ChangePasswordAsync(ChangePasswordVM changePasswordVM)
      {

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

         // CURRENT USER NAME
         var userName = "TODO";

         // LOCATE USER
         var userCursor = await _Collection.FindAsync($"{{'UserName':'{ userName}'}}");
         if (userCursor == null)
            return new BadRequestObjectResult(new string[] { WARNINGS.AUTHENTICATION_HAS_FAILED });
         var user = await userCursor.FirstOrDefaultAsync();
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
      Task<IActionResult> ChangePasswordAsync(ChangePasswordVM changePasswordVM);
   }

   partial struct WARNINGS
   {
      internal const string INVALID_CHANGEPASSWORD_PARAMETER = "WARNING_IDENTITY_INVALID_CHANGEPASSWORD_PARAMETER";
   }

}
