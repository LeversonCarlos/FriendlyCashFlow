using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Users
{

   partial class UsersService
   {

      public async Task<ActionResult<UserVM>> CreateDataAsync(CreateVM value)
      {
         try
         {

            // VALIDATE PASSWORD COMPLEXITY
            var passwordMessages = this.ValidatePassword(value.Password);
            if (passwordMessages.Length != 0) { return this.WarningResponse(passwordMessages); }

            // VALIDATE DUPLICITY
            if (await this.dbContext.Users.CountAsync(x => x.UserName == value.UserName) != 0)
            { return this.WarningResponse(this.GetTranslation("USERS_USER_NAME_ALREADY_EXISTS_WARNING")); }

            // NEW MODEL
            var data = new UserData()
            {
               UserName = value.UserName,
               Text = value.Description,
               JoinDate = DateTime.Now,
               RowStatus = (short)Base.enRowStatus.Temporary
            };

            // HASH THE PASSWORD
            var cryptService = this.GetService<Helpers.Crypt>();
            data.PasswordHash = cryptService.Encrypt(value.Password);

            // APPLY
            data.UserID = System.Guid.NewGuid().ToString();
            await this.dbContext.Users.AddAsync(data);
            await this.dbContext.SaveChangesAsync();

            // SEND ACTIVATION MAIL
            await this.SendActivationMailAsync(data.UserID);

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

   partial class UserController
   {

      [HttpPost("")]
      [AllowAnonymous]
      public async Task<ActionResult<UserVM>> CreateDataAsync([FromBody]CreateVM value)
      {
         var service = this.GetService<UsersService>();
         return await service.CreateDataAsync(value);
      }

   }

}
