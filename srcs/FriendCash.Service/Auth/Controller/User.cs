#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
#endregion

namespace FriendCash.Auth
{
   partial class AuthController
   {

      #region GetUsers
      [Authorize(Roles = "Admin")]
      [Route("users")]
      public IHttpActionResult GetUsers()
      {
         return Ok(this.UserManager.Users.ToList().Select(u => Model.Convert.ToView(u, this)));
      }
      #endregion

      #region GetUserByID
      [Authorize]
      [Route("user/{id:guid}", Name = "GetUserByID")]
      public async Task<IHttpActionResult> GetUserByID(string Id)
      {
         var data = await this.UserManager.FindByIdAsync(Id);

         if (data != null)
         {
            return Ok(Model.Convert.ToView(data, this));
         }

         return NotFound();

      }
      #endregion

      #region GetUserByName
      [Authorize]
      [Route("user/{username}/")]
      public async Task<IHttpActionResult> GetUserByName(string username)
      {
         var user = await this.UserManager.FindByNameAsync(username);

         if (user != null)
         {
            return Ok(Model.Convert.ToView(user, this));
         }

         return NotFound();

      }
      #endregion


      #region CreateUser
      [AllowAnonymous]
      [Route("user/create")]
      public async Task<IHttpActionResult> CreateUser(Model.viewCreateUser value)
      {
         try
         {

            // VALIDATE
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // CREATE
            var data = new Model.bindUser()
            {
               UserName = value.Email,
               Email = value.Email,
               FullName = value.Fullname,
               JoinDate = DateTime.UtcNow,
               ExpirationDate = DateTime.UtcNow.AddMonths(1)
            };

            // APPLY
            var applyResult = await this.UserManager.CreateAsync(data, value.Password);
            if (!applyResult.Succeeded) { return GetErrorResult(applyResult); }

            // ROLES
            await this.UserManager.AddToRolesAsync(data.Id, new string[] { "Viewer" });

            // EMAIL
            string emailCode = await this.UserManager.GenerateEmailConfirmationTokenAsync(data.Id);
            var emailHostAddress = this.GetWebHostAddress();
            emailCode = System.Uri.EscapeDataString(emailCode);
            var emailHostPath = string.Format("{0}Auth/Activate?id={1}&code={2}", emailHostAddress, data.Id, emailCode);
            var emailSubject = this.GetTranslation("LABEL_AUTH_REGISTER_ACTIVATE_EMAIL_SUBJECT");
            var emailBody =  this.GetTranslation("LABEL_AUTH_REGISTER_ACTIVATE_EMAIL_BODY");
            emailBody = string.Format(emailBody, emailHostPath);
            await this.UserManager.SendEmailAsync(data.Id, emailSubject, emailBody);

            // RESULT
            var locationHeader = new Uri(Url.Link("GetUserByID", new { id = data.Id }));
            return Created(locationHeader, Model.Convert.ToView(data, this));

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region CreateUserConfirm
      [AllowAnonymous]
      [HttpGet]
      [Route("CreateConfirm", Name = "CreateUserConfirmRoute")]
      public async Task<IHttpActionResult> CreateUserConfirm(string id = "", string code = "")
      {
         try
         { 

            // VALIDATE
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(code))
            {
               ModelState.AddModelError("", "User Id and Code are required");
               return BadRequest(ModelState);
            }

            // EXECUTE
            var confirmResult = await this.UserManager.ConfirmEmailAsync(id, code);
            if (!confirmResult.Succeeded) { return GetErrorResult(confirmResult); }

            // ROLES
            await this.UserManager.AddToRolesAsync(id, new string[] { "User" });
            return Ok(true); 

         }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region ChangePassword
      [Authorize] 
      [Route("ChangePassword")]
      public async Task<IHttpActionResult> ChangePassword(Model.viewChangePassword value)
      {
         try
         {

            // VALIDATE
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // EXECUTE
            var result = await this.UserManager.ChangePasswordAsync(this.GetUserID(), value.OldPassword, value.NewPassword);
            if (!result.Succeeded) { return GetErrorResult(result); }

            // RESULT
            return Ok(true);
         }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region DeleteUser
      [Authorize(Roles = "Admin")]
      [Route("user/{id:guid}")]
      public async Task<IHttpActionResult> DeleteUser(string id)
      {
         try
         {

            // LOCATE
            var data = await this.UserManager.FindByIdAsync(id);
            if (data == null) { return NotFound(); }

            // EXECUTE
            var execResult = await this.UserManager.DeleteAsync(data);
            if (!execResult.Succeeded) { return GetErrorResult(execResult); }

            // REMOVE DATA
            await this.DeleteUserData_Execute(id);

            // RESULT
            return Ok();

         }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region DeleteUserData

      [HttpDelete]
      [Authorize(Roles = "User")]
      [Route("deleteUserData")]
      public async Task<IHttpActionResult> DeleteUserData_ByUserName()
      {
         var id = this.GetUserID();
         return await this.DeleteUserData_Execute(id);
      }

      private async Task<IHttpActionResult> DeleteUserData_Execute(string id)
      {
         try
         {

            // ENTRIES
            using (var dataController = new Service.Entries.EntryController())
            { await dataController.RemoveUserData(id); }

            // PATTERNS
            using (var dataController = new Service.Entries.PatternController())
            { await dataController.RemoveUserData(id); }

            // RECURRENCY
            using (var dataController = new Service.Entries.RecurrencyController())
            { await dataController.RemoveUserData(id); }

            // BALANCE
            using (var dataController = new Service.Balances.BalanceController())
            { await dataController.RemoveUserData(id); }

            // CATEGORIES
            using (var dataController = new Service.Categories.CategoryController())
            { await dataController.RemoveUserData(id); }

            // ACCOUNTS
            using (var dataController = new Service.Accounts.AccountController())
            { await dataController.RemoveUserData(id); }

            // RESULT
            return Ok(true);

         }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }

      #endregion


      #region AddToRole
      [Authorize(Roles = "Admin")]
      [Route("addToRole/{id:guid}/{role}", Name = "AddToRole")]
      public async Task<IHttpActionResult> AddToRole(string id, string role)
      {

         // LOCATE DATA
         var userData = await this.UserManager.FindByIdAsync(id);
         if (userData == null) { return NotFound(); }
         var roleData = await this.RoleManager.FindByNameAsync(role);
         if (roleData == null) { return NotFound(); }

         // VALIDATE
         var userRoles = await this.UserManager.GetRolesAsync(userData.Id);
         if (userRoles.Contains(roleData.Name)) { return Ok(); }

         // APPLY ROLE
         var result = await this.UserManager.AddToRoleAsync(userData.Id, roleData.Name);
         if (!result.Succeeded)
         {
            var msg = "";
            result.Errors.ToList().ForEach(x => msg += x + Environment.NewLine);
            return BadRequest(msg);
         }

         // RESULT
         return Ok();
      }
      #endregion

      #region RemoveFromRole
      [Authorize(Roles = "Admin")]
      [Route("removeFromRole/{id:guid}/{role}", Name = "RemoveFromRole")]
      public async Task<IHttpActionResult> RemoveFromRole(string id, string role)
      {

         // LOCATE USER
         var userData = await this.UserManager.FindByIdAsync(id);
         if (userData == null) { return NotFound(); }
         var roleData = await this.RoleManager.FindByNameAsync(role);
         if (roleData == null) { return NotFound(); }

         // VALIDATE
         var userRoles = await this.UserManager.GetRolesAsync(userData.Id);
         if (!userRoles.Contains(roleData.Name)) { return Ok(); }

         // APPLY ROLE
         var result = await this.UserManager.RemoveFromRoleAsync(userData.Id, roleData.Name);
         if (!result.Succeeded)
         {
            var msg = "";
            result.Errors.ToList().ForEach(x => msg += x + Environment.NewLine);
            return BadRequest(msg);
         }

         // RESULT
         return Ok();
      }
      #endregion

   }
}
