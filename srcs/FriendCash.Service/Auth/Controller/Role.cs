#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
#endregion

namespace FriendCash.Auth
{
   partial class AuthController 
   {

      #region GetRoles
      [Authorize(Roles = "Admin")]
      [Route("roles")]
      public IHttpActionResult GetRoles()
      {
         return Ok(this.RoleManager.Roles.ToList().Select(u => Model.Convert.ToView(u, this)));
      }
      #endregion

      #region GetRoleByID
      [Authorize(Roles = "Admin")]
      [Route("role/{id:guid}", Name = "GetRoleByID")]
      public async Task<IHttpActionResult> GetRoleByID(string Id)
      {
         var data = await this.RoleManager.FindByIdAsync(Id);

         if (data != null)
         {
            return Ok(Model.Convert.ToView(data, this));
         }

         return NotFound();

      }
      #endregion


      #region CreateRole
      [Authorize(Roles = "Admin")]
      [Route("role/create")]
      public async Task<IHttpActionResult> CreateRole(Model.viewCreateRole value)
      {
         try
         {

            // VALIDATE
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // CREATE
            var data = new IdentityRole() { Name = value.Name };

            // APPLY
            var applyResult = await this.RoleManager.CreateAsync(data);
            if (!applyResult.Succeeded) { return GetErrorResult(applyResult); }

            // RESULT
            var locationHeader = new Uri(Url.Link("GetRoleByID", new { id = data.Id }));
            return Created(locationHeader, Model.Convert.ToView(data, this));

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region DeleteRole
      [Authorize(Roles = "Admin")]
      [Route("role/{id:guid}")]
      public async Task<IHttpActionResult> DeleteRole(string id)
      {
         try
         {

            // LOCATE
            var data = await this.RoleManager.FindByIdAsync(id);
            if (data == null) { return NotFound(); }

            // EXECUTE
            var execResult = await this.RoleManager.DeleteAsync(data);
            if (!execResult.Succeeded) { return GetErrorResult(execResult); }

            // RESULT
            return Ok();

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region ManageUsersInRole
      [Route("role/ManageUsers")]
      public async Task<IHttpActionResult> ManageUsersInRole(Model.viewRoleUsers value)
      {
         try
         {

            // LOCATE ROLE
            var role = await this.RoleManager.FindByIdAsync(value.ID);
            if (role == null) { ModelState.AddModelError("", "Role does not exist"); return BadRequest(ModelState); }

            // ENROLLED USERS
            foreach (string user in value.EnrolledUsers)
            {

               // LOCATE USER
               var appUser = await this.UserManager.FindByIdAsync(user);
               if (appUser == null) { ModelState.AddModelError("", String.Format("User: {0} does not exists", user)); continue; }

               // CHECK USER IN ROLE AND APPLY IF NEED
               if (!this.UserManager.IsInRole(user, role.Name))
               {
                  IdentityResult addResult = await this.UserManager.AddToRoleAsync(user, role.Name);
                  if (!addResult.Succeeded) { ModelState.AddModelError("", String.Format("User: {0} could not be added to role", user)); }
               }
            }

            // REMOVED USERS
            foreach (string user in value.RemovedUsers)
            {

               // LOCATE USER
               var appUser = await this.UserManager.FindByIdAsync(user);
               if (appUser == null) { ModelState.AddModelError("", String.Format("User: {0} does not exists", user)); continue; }

               // REMOVE USER FROM ROLE
               IdentityResult removeResult = await this.UserManager.RemoveFromRoleAsync(user, role.Name);
               if (!removeResult.Succeeded) { ModelState.AddModelError("", String.Format("User: {0} could not be removed from role", user)); }
            }

            // RESULT
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            return Ok();

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

   }
}
