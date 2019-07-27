#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
#endregion

namespace FriendCash.Web.Controllers
{
   [RoutePrefix("home")]
   public class HomeController : Controllers.Base
   {

      #region Index
      public async Task<ActionResult> Index()
      {
         if (!this.User.Identity.IsAuthenticated) { return RedirectToAction("login", "auth"); }
         return View();
      }
      #endregion

      #region GetTranslation
      [Route("translation/{key}")]
      public async Task<string> GetTranslation(string key)
      { return await this.GetTranslationByKeyAsync(key); }
      #endregion

   }
}