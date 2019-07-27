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

      #region GetSignatures
      [Authorize(Roles = "Admin")]
      [HttpGet, Route("signatures/{id:guid}", Name = "GetSignaturesByUser")]
      public async Task<IHttpActionResult> GetSignatures(string id)
      {
         var query = this.authContext.Signatures
            .Where(x => x.idUser == id)
            .OrderByDescending(x => x.DueDate)
            .AsQueryable();
         var data = await Task.FromResult(query.ToList());
         return Ok(data.Select(u => Model.Convert.ToView(u, this)));
      }
      #endregion

      #region GetSignature
      internal async Task<Model.bindSignature> GetSignature(string idUser)
      {
         var query = this.authContext.Signatures
            .Where(x => x.idUser == idUser && x.DueDate >= DateTime.Now)
            .OrderBy(x=> x.DueDate)
            .Take(1)
            .AsQueryable();
         var data = await Task.FromResult(query.FirstOrDefault());
         return data;
      }
      #endregion


      #region CreateSignature
      [Authorize(Roles = "Admin")]
      [HttpPost, Route("signature")]
      public async Task<IHttpActionResult> CreateSignature(Model.editSignature value)
      {
         try
         {

            // VALIDATE
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // LAST SIGNATURE
            var dueDate = DateTime.Now.AddDays(value.days);
            var lastQuery = this.authContext.Signatures
               .Where(x => x.idUser == value.idUser)
               .OrderByDescending(x => x.DueDate)
               .Take(1)
               .AsQueryable();
            var lastData = await Task.FromResult(lastQuery.FirstOrDefault());
            if (lastData != null && lastData.DueDate >= DateTime.Now)
            { dueDate = lastData.DueDate.AddDays(value.days); }

            // CREATE
            var data = new Model.bindSignature()
            {
               idUser = value.idUser,
               DueDate = dueDate
            };

            // APPLY
            this.authContext.Signatures.Add(data);
            var iSave = await this.authContext.SaveChangesAsync();
            if (iSave == 0) { throw new Exception("Data wasnt saved"); }
            await this.ApplyExpiration(value.idUser);

            // RESULT
            // var locationHeader = new Uri(Url.Link("GetSignaturesByUser", data.idUser));
            var locationHeader = new Uri("http://127.0.0.1");
            return Created(locationHeader, Model.Convert.ToView(data, this));

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region DeleteSignature
      [Authorize(Roles = "Admin")]
      [HttpDelete, Route("signature/{idUser:guid}/{id:long}")]
      public async Task<IHttpActionResult> DeleteSignature(string idUser, long id)
      {
         try
         {

            // VALIDATE MODEL
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // LOCATE
            var dataQuery = this.authContext.Signatures
               .Where(x => x.idUser == idUser && x.idSignature == id)
               .AsQueryable();
            var dataModel = await Task.FromResult(dataQuery.FirstOrDefault());
            if (dataModel == null) { ModelState.AddModelError("WarningMessage", this.GetTranslation("WARNING_SIGNATURE_NOTFOUND")); return BadRequest(ModelState); }

            // APPLY
            // dataModel.RowStatus = Base.BaseModel.enRowStatus.Removed;
            this.authContext.Entry(dataModel).State = System.Data.Entity.EntityState.Deleted;
            await this.authContext.SaveChangesAsync();
            await this.ApplyExpiration(idUser);

            // RESULT
            return Ok(dataModel);

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region ApplyExpiration
      private async Task ApplyExpiration(string idUser)
      {
         try
         {

            // INITIALIZE
            var expirationDate = DateTime.Now.AddMinutes(5);

            // SIGNATURE
            var signatureQuery = this.authContext.Signatures
               .Where(x => x.idUser == idUser)
               .OrderByDescending(x => x.DueDate)
               .Take(1)
               .AsQueryable();
            var signatureData = await Task.FromResult(signatureQuery.FirstOrDefault());
            if (signatureData != null && signatureData.DueDate >= expirationDate)
            { expirationDate = signatureData.DueDate; }

            // USER
            var userQuery = this.authContext.Users
               .Where(x => x.Id == idUser)
               .AsQueryable();
            var userData = await Task.FromResult(userQuery.FirstOrDefault());
            if (userData == null) { return; }

            // APPLY
            userData.ExpirationDate = expirationDate;
            var iSave = await this.authContext.SaveChangesAsync();
            if (iSave == 0) { throw new Exception("Data wasnt saved"); }
            return;

         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

   }
}
