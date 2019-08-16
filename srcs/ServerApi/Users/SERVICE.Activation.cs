
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FriendlyCashFlow.API.Users
{
   partial class UsersService
   {

      public async Task<ActionResult<bool>> SendActivationMailAsync(string userID)
      {
         try
         {

            // LOAD DATA
            var data = await this.GetDataQuery().Where(x => x.UserID == userID).FirstOrDefaultAsync();
            if (data == null) { return this.NotFoundResponse(); }

            // ACTIVATION LINK
            var appSettings = this.GetService<IOptions<AppSettings>>().Value;
            var cryptService = this.GetService<Helpers.Crypt>();
            var activationCode = $"{data.UserID}-{data.UserName}-{data.JoinDate.ToString("yyyyMMdd-HHmmss")}";
            var mailBodyCommandLink = $"{appSettings.BaseHost}/api/users/activate/{data.UserID}/{cryptService.Encrypt(activationCode)}";

            // SEND ACTIVATION MAIL
            var mailService = this.GetService<Helpers.Mail>();
            var mailSubject = string.Format(this.GetTranslation("USERS_ACTIVATION_MAIL_SUBJECT"), "Cash Flow");
            var mailBodyTitle = string.Format(this.GetTranslation("USERS_ACTIVATION_MAIL_BODY_TITLE"), "Cash Flow");
            var mailBodyMessage = string.Format(this.GetTranslation("USERS_ACTIVATION_MAIL_BODY_MESSAGE"), "Cash Flow");
            var mailBodyCommandText = this.GetTranslation("USERS_ACTIVATION_MAIL_BODY_COMMAND");
            var mailBody = string.Format(this.CreateDataAsync_GetMailBody(), mailBodyTitle, mailBodyMessage, mailBodyCommandLink, mailBodyCommandText);
            await mailService.SendAsync(mailSubject, mailBody, data.UserName);

            return true;
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

      public async Task<ActionResult<UserVM>> ActivateUserAsync(string userID, string activationCode)
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
