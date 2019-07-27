#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
#endregion

namespace FriendCash.Service.Base
{
   partial class BaseController
   {

      #region GetTranslationAsync

      protected Task<string> GetTranslationAsync(string idTranslation)
      {
         var idLanguage = this.Request.Headers.AcceptLanguage.Select(x => x.Value).FirstOrDefault();
         return this.GetTranslationAsync(idLanguage, idTranslation);
      }

      protected Task<string> GetTranslationAsync(string idLanguage, string idTranslation)
      {
         /*
         var sResult = "";
         var oTask = Task.Run(() => { sResult = this.GetTranslation(idLanguage, idTranslation); });
         oTask.Wait();
         return sResult;
         */
         return Task.FromResult<string>(BaseController.GetTranslation(idLanguage, idTranslation));
      }

      #endregion

      #region GetTranslation
      protected string GetTranslation(string idTranslation)
      {
         try
         {
            var idLanguage = this.Request.Headers.AcceptLanguage.Select(x => x.Value).FirstOrDefault();
            return BaseController.GetTranslation(idLanguage, idTranslation);
         }
         catch { return idTranslation; }
      }
      #endregion 

      #region GetTranslationStatic
      internal static string GetTranslation(string idLanguage, string idTranslation)
      {
         try
         {

            // CONTEXT
            using (var oContext = new Base.dbContext())
            {
               var sResult = string.Empty;

               // SPECIFIC LANGUAGE
               sResult = oContext.Translations
                  .Where(x => x.idLanguage == idLanguage && x.idTranslation == idTranslation)
                  .Select(x => x.Text)
                  .FirstOrDefault();
               if (!string.IsNullOrEmpty(sResult)) { return sResult; }

               // PARENT LANGUAGE
               sResult = oContext.Translations
                  .Where(x => x.idLanguage == idLanguage.Substring(0, 2) && x.idTranslation == idTranslation)
                  .Select(x => x.Text)
                  .FirstOrDefault();
               if (!string.IsNullOrEmpty(sResult)) { return sResult; }

               // DEFAULT LANGUAGE
               sResult = oContext.Translations
                  .Where(x => x.idLanguage == "en-US" && x.idTranslation == idTranslation)
                  .Select(x => x.Text)
                  .FirstOrDefault();
               if (!string.IsNullOrEmpty(sResult)) { return sResult; }

               // OTHERWISE
               return idTranslation;

            };

         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

   }
}