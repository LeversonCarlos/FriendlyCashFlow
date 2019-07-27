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

      #region GetClients
      [Authorize(Roles = "Admin")]
      [HttpGet, Route("clients")]
      public IHttpActionResult GetClients()
      {
         return Ok(this.authContext.Clients.ToList().Select(u => Model.Convert.ToView(u, this)));
      }
      #endregion

      #region GetClientByID

      [Authorize(Roles = "Admin")]
      [HttpGet, Route("client/{id:guid}", Name = "GetClientByID")]
      public async Task<IHttpActionResult> GetClientByID(string Id)
      {
         var data = await Task.FromResult(GetClient(Id));

         if (data != null)
         {
            return Ok(Model.Convert.ToView(data, this));
         }

         return NotFound();

      }

      internal Model.bindClient GetClient(string Id)
      {
         var data = this.authContext.Clients.Where(x => x.Id == Id).FirstOrDefault();
         return data;
      }

      #endregion


      #region CreateClient
      [Authorize(Roles = "Admin")]
      [HttpPost, Route("client")]
      public async Task<IHttpActionResult> CreateClient(Model.editClient value)
      {
         try
         {

            // VALIDATE
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // CREATE
            var data = new Model.bindClient()
            {
               Name = value.Name,
               Type = value.Type,
               RefreshTokenLifetime = value.RefreshTokenLifetime,
               AllowedOrigin = value.AllowedOrigin,
               Secret = value.Secret,
               Active = value.Active
            };

            // APPLY
            this.authContext.Clients.Add(data);
            var iSave = await this.authContext.SaveChangesAsync();
            if (iSave == 0) { throw new Exception("Data wasnt saved"); }

            // RESULT
            var locationHeader = new Uri(Url.Link("GetClientByID", new { id = data.Id }));
            return Created(locationHeader, Model.Convert.ToView(data, this));

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region UpdateClient
      [Authorize(Roles = "Admin")]
      [HttpPut, Route("client")]
      public async Task<IHttpActionResult> UpdateClient(Model.editClient value)
      {
         try
         {

            // VALIDATE MODEL
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // LOCATE
            var data = await Task.FromResult(this.authContext.Clients.Where(x => x.Id == value.ID).FirstOrDefault());
            if (data == null) { return NotFound(); }

            // APPLY
            data.Name = value.Name;
            data.Type = value.Type;
            data.RefreshTokenLifetime = value.RefreshTokenLifetime;
            data.AllowedOrigin = value.AllowedOrigin;
            data.Secret = value.Secret;
            data.Active = value.Active;
            var iSave = await this.authContext.SaveChangesAsync();
            if (iSave == 0) { throw new Exception("Data wasnt saved"); }

            // RESULT
            return Ok(Model.Convert.ToView(data, this));

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region DeleteClient
      [Authorize(Roles = "Admin")]
      [HttpDelete, Route("client/{id:guid}")]
      public async Task<IHttpActionResult> DeleteClient(string id)
      {
         try
         {

            // LOCATE
            var data = await Task.FromResult(this.authContext.Clients.Where(x => x.Id == id).FirstOrDefault());
            if (data == null) { return NotFound(); }

            // APPLY
            this.authContext.Clients.Remove(data);
            var iSave = await this.authContext.SaveChangesAsync();
            if (iSave == 0) { throw new Exception("Data wasnt saved"); }

            // RESULT
            return Ok();

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

   }
}
