
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Users
{
   partial class UsersService
   {

      public async Task<ActionResult<UserVM>> ActivateDataAsync(string userID, string activationCode)
      {
         try
         {

            // LOCATE USER
            var data = await this.GetDataQuery().Where(x => x.UserID == userID).FirstOrDefaultAsync();
            if (data == null)
            { return this.WarningResponse(this.GetTranslation("USERS_NOT_FOUND_WARNING")); }

            // ACTIVATION CODE
            var cryptService = this.GetService<Helpers.Crypt>();
            var userActivationCode = cryptService.Encrypt($"{data.UserID}-{data.UserName}-{data.JoinDate.ToString("yyyyMMdd-HHmmss")}");
            if (userActivationCode != activationCode)
            { return this.WarningResponse(this.GetTranslation("USERS_INVALID_ACTIVATION_CODE_WARNING")); }

            // APPLY
            await this.dbContext.UserRoles.AddAsync(new UserRoleData { UserID = data.UserID, RoleID = UserRoleEnum.Viewer.ToString() });
            await this.dbContext.SaveChangesAsync();

            // RESULT
            var result = UserVM.Convert(data);
            return this.CreatedResponse("users", result.UserID, result);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }
}
