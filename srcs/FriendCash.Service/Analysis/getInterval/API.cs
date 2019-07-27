#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
#endregion

namespace FriendCash.Service.Analysis
{
   partial class AnalysyController
   {

      #region GetInterval
      [Authorize(Roles = "User,Viewer")]
      [Route("interval/{year}/{month}")]
      public IHttpActionResult GetInterval(int year, short month)
      {
         try
         {
            var resultData = new Model.viewInterval();

            // CULTURE INFO
            var acceptLanguage = this.Request.Headers.AcceptLanguage.Select(x => x.Value).FirstOrDefault();
            var cultureInfo = new System.Globalization.CultureInfo(acceptLanguage);

            // INTERVAL
            var currentDate = new DateTime(year, month, 1);
            resultData.CurrentMonth = new Model.viewParam(currentDate, cultureInfo);
            resultData.PreviousMonth = new Model.viewParam(currentDate.AddMonths(-1), cultureInfo);
            resultData.NextMonth = new Model.viewParam(currentDate.AddMonths(1), cultureInfo);

            return Ok(resultData);

         }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

   }
}
