using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
            var data = await this.dbContext.Users.Where(x => x.UserID == userID).FirstOrDefaultAsync();
            if (data == null) { return this.NotFoundResponse(); }
            if (data.RowStatus != (short)Base.enRowStatus.Temporary) { return this.WarningResponse(this.GetTranslation("USERS_USER_ALREADY_ACTIVATED_WARNING")); }

            // ACTIVATION LINK
            var appSettings = this.GetService<IOptions<AppSettings>>().Value;
            var cryptService = this.GetService<Helpers.Crypt>();
            var activationCode = $"{data.UserID}-{data.UserName}-{data.JoinDate.ToString("yyyyMMdd-HHmmss")}";
            activationCode = cryptService.Encrypt(activationCode);
            activationCode = activationCode.Replace("+", "").Replace("/", "").Replace("\\", "").Replace("&", "");
            var mailBodyCommandLink = $"{appSettings.BaseHost}/activate/{data.UserID}/{activationCode}";

            // SEND ACTIVATION MAIL
            var mailService = this.GetService<Helpers.Mail>();
            var mailSubject = string.Format(this.GetTranslation("USERS_ACTIVATION_MAIL_SUBJECT"), "Cash Flow");
            var mailBodyTitle = string.Format(this.GetTranslation("USERS_ACTIVATION_MAIL_BODY_TITLE"), data.Text, "Cash Flow");
            var mailBodyMessage = this.GetTranslation("USERS_ACTIVATION_MAIL_BODY_MESSAGE");
            var mailBodyCommandText = this.GetTranslation("USERS_ACTIVATION_MAIL_BODY_COMMAND");
            var mailBody = string.Format(this.CreateDataAsync_GetMailBody(), mailBodyTitle, mailBodyMessage, mailBodyCommandLink, mailBodyCommandText);
            await mailService.SendAsync(mailSubject, mailBody, data.UserName);

            return true;
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

      public async Task<ActionResult<bool>> ActivateUserAsync(string userID, string activationCode)
      {
         try
         {

            // LOCATE USER
            var data = await this.dbContext.Users.Where(x => x.UserID == userID).FirstOrDefaultAsync();
            if (data == null) { return this.WarningResponse(this.GetTranslation("USERS_USER_NOT_FOUND_WARNING")); }
            if (data.RowStatus != (short)Base.enRowStatus.Temporary) { return this.WarningResponse(this.GetTranslation("USERS_USER_ALREADY_ACTIVATED_WARNING")); }

            // ACTIVATION CODE
            var cryptService = this.GetService<Helpers.Crypt>();
            var userActivationCode = cryptService.Encrypt($"{data.UserID}-{data.UserName}-{data.JoinDate.ToString("yyyyMMdd-HHmmss")}");
            userActivationCode = userActivationCode.Replace("+", "").Replace("/", "").Replace("\\", "").Replace("&", "");
            if (userActivationCode != activationCode)
            {
               this.TrackEvent("Invalid Activation Code", $"userID:{userID}", $"activationCode:{activationCode}", $"userActivationCode:{userActivationCode}");
               return this.WarningResponse(this.GetTranslation("USERS_INVALID_ACTIVATION_CODE_WARNING"));
            }

            // GENERATE RESOURCE
            var resource = new UserResourceData
            {
               UserID = data.UserID,
               ResourceID = System.Guid.NewGuid().ToString(),
               RowStatus = (short)Base.enRowStatus.Active
            };
            await this.dbContext.UserResources.AddAsync(resource);

            // USER ROLES
            var userRoles = (new UserRoleEnum[] { UserRoleEnum.Viewer, UserRoleEnum.Editor, UserRoleEnum.Owner })
               .Select(x => new UserRoleData
               {
                  UserID = data.UserID,
                  ResourceID = resource.ResourceID,
                  RoleID = x.ToString()
               })
               .ToList();

            // APPLY
            data.SelectedResourceID = resource.ResourceID;
            data.RowStatus = (short)Base.enRowStatus.Active;
            await this.dbContext.UserRoles.AddRangeAsync(userRoles);
            await this.dbContext.SaveChangesAsync();

            // RESULT
            var result = UserVM.Convert(data);
            return this.OkResponse(true);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

   partial class UserController
   {

      [HttpPost("sendActivation/{userID}")]
      [AllowAnonymous]
      public async Task<ActionResult<bool>> SendActivationMailAsync(string userID)
      {
         var service = this.GetService<UsersService>();
         return await service.SendActivationMailAsync(userID);
      }

      [HttpGet("activate/{userID}/{activationCode}")]
      [AllowAnonymous]
      public async Task<ActionResult<bool>> ActivateUserAsync(string userID, string activationCode)
      {
         var service = this.GetService<UsersService>();
         return await service.ActivateUserAsync(userID, activationCode);
      }

   }

}
