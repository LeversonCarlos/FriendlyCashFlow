#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
#endregion

namespace FriendCash.Service.Translation
{

   [RoutePrefix("api/translation")]
   public class TranslationController : Base.BaseController
   {

      /*
      #region GetTranslationByID
      [AllowAnonymous]
      [Route("{key}/{l}", Name = "GetTranslationAnonymous")]
      public async Task<IHttpActionResult> GetTranslationAnonymous(string key, string l)
      {
         try
         {

            // VALIDATE
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // TRANSLATION
            var sResult = await this.GetTranslationAsync(l, key);
            return Ok(sResult);

         }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion
      */ 

      #region GetTranslationByKey
      [AllowAnonymous] //Authorize
      [Route("{key}", Name = "GetTranslationByKey")]
      public async Task<IHttpActionResult> GetTranslationByKey(string key)
      {
         try
         {

            // VALIDATE
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // LANGUAGE
            var idLanguage = this.Request.Headers.AcceptLanguage.Select(x => x.Value).FirstOrDefault();

            // TRANSLATION
            var sResult = await this.GetTranslationAsync(idLanguage, key);
            return Ok(sResult);

         }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

   }
}