
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FriendlyCashFlow.API.Users
{
   partial class UsersService
   {

      public async Task<ActionResult<UserVM>> CreateDataAsync(CreateVM value)
      {
         try
         {

            // VALIDATE EMAIL FORMAT
            // TODO

            // VALIDATE PASSWORD COMPLEXITY
            var passwordMessages = this.ValidatePassword(value.Password);
            if (passwordMessages.Length != 0) { return this.WarningResponse(passwordMessages); }

            // VALIDATE DUPLICITY
            if (await this.GetDataQuery().CountAsync(x => x.UserName == value.UserName) != 0)
            { return this.WarningResponse(this.GetTranslation("USERS_USER_NAME_ALREADY_EXISTS_WARNING")); }

            // NEW MODEL
            var data = new UserData()
            {
               UserName = value.UserName,
               Text = value.UserName.Split("@", StringSplitOptions.RemoveEmptyEntries)[0],
               JoinDate = DateTime.Now,
               RowStatus = 1
            };

            // HASH THE PASSWORD
            var cryptService = this.GetService<Helpers.Crypt>();
            data.PasswordHash = cryptService.Encrypt(value.Password);

            // APPLY
            data.UserID = System.Guid.NewGuid().ToString();
            await this.dbContext.Users.AddAsync(data);
            await this.dbContext.SaveChangesAsync();

            // ACTIVATION LINK
            var appSettings = this.GetService<IOptions<AppSettings>>().Value;
            var mailBodyCommandLink = $"{appSettings.BaseHost}/api/users/activate/{data.UserID}";

            // SEND ACTIVATION MAIL
            var mailService = this.GetService<Helpers.Mail>();
            var mailSubject = string.Format(this.GetTranslation("USERS_ACTIVATION_MAIL_SUBJECT"), "Cash Flow");
            var mailBodyTitle = string.Format(this.GetTranslation("USERS_ACTIVATION_MAIL_BODY_TITLE"), "Cash Flow");
            var mailBodyMessage = string.Format(this.GetTranslation("USERS_ACTIVATION_MAIL_BODY_MESSAGE"), "Cash Flow");
            var mailBodyCommandText = this.GetTranslation("USERS_ACTIVATION_MAIL_BODY_COMMAND");
            var mailBody = string.Format(this.CreateDataAsync_GetMailBody(), mailBodyTitle, mailBodyMessage, mailBodyCommandLink, mailBodyCommandText);
            await mailService.SendAsync(mailSubject, mailBody, data.UserName);

            // RESULT
            var result = UserVM.Convert(data);
            return this.CreatedResponse("users", result.UserID, result);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

      private string CreateDataAsync_GetMailBody()
      {
         return @"
         <html>
         <body>
            <div style='margin-bottom:50px;'>
               <h3>{0}</h3>
               <p>{1}</p>
               <a href='{2}' style='display:inline-block; margin:20px auto;padding: 10px 20px;border:1px solid;text-decoration:none;background-color:#f5f5f5;border-radius:4px;'>{3}</a>
            </div>
         </body>
         </html>
         ";
      }

   }
}
