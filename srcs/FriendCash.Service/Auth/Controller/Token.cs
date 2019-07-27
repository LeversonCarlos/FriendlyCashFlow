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

      #region GetTokens
      [Authorize(Roles = "Admin")]
      [HttpGet, Route("tokens")]
      public IHttpActionResult GetTokens()
      {
         return Ok(this.authContext.Tokens.ToList());
      }
      #endregion

      #region GetTokenByID
      public async Task<Model.bindToken> GetTokenByID(string Id)
      {

         //LOCATE
         var data = await Task.FromResult(this.authContext.Tokens.Where(x => x.Id == Id).FirstOrDefault());

         // RETURN
         return data;

      }
      #endregion


      #region AddToken
      public async Task<bool> AddToken(Model.bindToken value)
      {
         try
         {

            // REMOVE EXPIRED
            var expiredList = this.authContext.Tokens
               .Where(x => 
                  x.ClientId == value.ClientId && 
                  x.UserName == value.UserName && 
                  x.ExpiryTime < DateTime.UtcNow)
               .ToList();
            if (expiredList != null && expiredList.Count != 0) {
               foreach (var expiredItem in expiredList)
               {
                  await this.RemoveToken(expiredItem);
               }
            }

            // ADD NEW TOKEN
            this.authContext.Tokens.Add(value);
            var iSave = await this.authContext.SaveChangesAsync();            

            // RESULT
            return iSave != 0;

         }
         catch (Exception ex) { throw ex; }
      }
      #endregion      

      #region RemoveToken

      public async Task<bool> RemoveToken(string id)
      {
         try
         {

            // LOCATE
            var data = await this.GetTokenByID(id);

            // APPLY
            return await this.RemoveToken(data);

         }
         catch (Exception ex) { throw ex; }
      }

      public async Task<bool> RemoveToken(Model.bindToken value)
      {
         try
         {

            // APPLY
            this.authContext.Tokens.Remove(value);
            var iSave = await this.authContext.SaveChangesAsync();

            // RESULT
            return iSave != 0;

         }
         catch (Exception ex) { throw ex; }
      }

      #endregion

   }
}
