#region Using
using FriendCash.Service.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
#endregion

namespace FriendCash.Web.Controllers
{
   [Authorize]
   public class DashboardController : Controllers.Base
   {

      #region Analysis
      public ActionResult Analysis()
      {
         return View();
      }
      #endregion

      #region GetBalance
      public async Task<ActionResult> GetBalance()
      {
         var apiURL = "api/dashboard/balance";
         return await this.GetApiResult<List<FriendCash.Service.Dashboard.Model.viewBalanceGroup>>(apiURL);

      }
      #endregion    

      #region GetPending
      public async Task<ActionResult> GetPending()
      {
         var apiURL = "api/entries/pending";
         return await this.GetApiResult<List<FriendCash.Service.Entries.Model.viewEntry>>(apiURL);
      }
      #endregion

   }
}