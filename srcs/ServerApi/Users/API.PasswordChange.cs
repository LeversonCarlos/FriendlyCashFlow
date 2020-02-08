using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Users
{

   partial class UsersService
   {

      public async Task<ActionResult<bool>> PasswordChangeAsync(PasswordChangeVM value)
      {
         try
         {
            var cryptService = this.GetService<Helpers.Crypt>();
            var userHelper = this.GetService<Helpers.User>();

            // CHECK OLD PASSWORD
            var oldPasswordHash = cryptService.Encrypt(value.OldPassword);
            var user = await this.dbContext.Users
               .Where(x =>
                  x.RowStatus != (short)Base.enRowStatus.Removed &&
                  x.UserID == userHelper.UserID &&
                  x.PasswordHash == oldPasswordHash)
               .FirstOrDefaultAsync();
            if (user == null) { return this.WarningResponse(this.GetTranslation("USERS_USER_NOT_FOUND_WARNING")); }

            // VALIDATE PASSWORD COMPLEXITY
            var passwordMessages = this.ValidatePassword(value.NewPassword);
            if (passwordMessages.Length != 0) { return this.WarningResponse(passwordMessages); }

            // HASH THE PASSWORD
            user.PasswordHash = cryptService.Encrypt(value.NewPassword);

            // APPLY
            await this.dbContext.SaveChangesAsync();

            // RESULT
            return this.OkResponse(true);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

   partial class UserController
   {

      [HttpPost("passwordChange")]
      [Authorize]
      public async Task<ActionResult<bool>> PasswordChangeAsync([FromBody]PasswordChangeVM value)
      {
         return await this.GetService<UsersService>().PasswordChangeAsync(value);
      }

   }

}
