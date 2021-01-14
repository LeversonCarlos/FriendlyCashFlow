using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Elesse.Identity
{

   partial class IdentityService
   {

      public async Task<IActionResult> RegisterAsync(RegisterVM registerVM)
      {

         // VALIDATE PARAMETERS
         if (registerVM == null)
            return new BadRequestObjectResult(new string[] { WARNINGS.INVALID_REGISTER_PARAMETER });

         // VALIDATE USERNAME
         var validateUsername = await ValidateUsernameAsync(registerVM.UserName);
         if (validateUsername.Length > 0)
            return new BadRequestObjectResult(validateUsername);

         // VALIDATE PASSWORD
         var validatePassword = await ValidatePasswordAsync(registerVM.Password);
         if (validatePassword.Length > 0)
            return new BadRequestObjectResult(validatePassword);

         // VALIDATE DUPLICITY
         var user = await _UserRepository.GetUserByUserNameAsync(registerVM.UserName);
         if (user != null)
            return new BadRequestObjectResult(new string[] { WARNINGS.USERNAME_ALREADY_USED });

         // ADD NEW USER
         user = new UserEntity(registerVM.UserName, registerVM.Password.GetHashedText(_Settings.PasswordSalt));
         await _UserRepository.AddUserAsync(user);

         // SEND ACTIVATION MAIL
         // TODO

         // TRACK EVENT
         _InsightsService.TrackEvent("Identity Service Register");

         // RESULT
         return new OkResult();
      }

   }

   partial interface IIdentityService
   {
      Task<IActionResult> RegisterAsync(RegisterVM registerVM);
   }

   partial struct WARNINGS
   {
      internal const string INVALID_REGISTER_PARAMETER = "WARNING_IDENTITY_INVALID_REGISTER_PARAMETER";
      internal const string USERNAME_ALREADY_USED = "WARNING_IDENTITY_USERNAME_ALREADY_USED";
   }

}
