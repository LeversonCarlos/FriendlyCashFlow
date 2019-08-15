
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

            // SEND CONFIRMATION MAIL
            var mailService = this.GetService<Helpers.Mail>();
            var mailSubject = this.GetTranslation("USERS_CONFIRMATION_MAIL_SUBJECT_TITLE");
            var mailBody = this.GetTranslation("USERS_CONFIRMATION_MAIL_BODY_TITLE");
            await mailService.SendAsync(mailSubject, mailBody, data.UserName);

            // RESULT
            var result = UserVM.Convert(data);
            return this.CreatedResponse("users", result.UserID, result);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }
}
