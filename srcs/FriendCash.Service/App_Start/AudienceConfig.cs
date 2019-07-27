#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
#endregion

namespace FriendCash.Service.Provider
{

   [RoutePrefix("api/audience")]
   public class AudienceController : Base.BaseController
   {

      #region CreateAudience
      [AllowAnonymous]
      [Route("create")]
      public async Task<IHttpActionResult> CreateAudience(AudienceModel value)
      {
         try
         {

            // VALIDATE
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // AUDIENCE ID
            value.AudienceID = Guid.NewGuid().ToString("N");

            // AUDIENCE SECRET
            var key = new byte[32];
            System.Security.Cryptography.RNGCryptoServiceProvider.Create().GetBytes(key);
            value.AudienceSecret = Microsoft.Owin.Security.DataHandler.Encoder.TextEncodings.Base64Url.Encode(key);

            // RESULT
            return Ok(value);

         }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

   }

   public class AudienceModel
   {
      public string Key { get; set; }
      public string AudienceID { get; set; }
      public string AudienceSecret { get; set; }
   }

}