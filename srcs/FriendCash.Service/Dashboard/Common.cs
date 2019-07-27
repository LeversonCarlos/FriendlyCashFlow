#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web.Http;
#endregion

namespace FriendCash.Service.Dashboard
{

   [RoutePrefix("api/dashboard")]
   public partial class DashboardController : Base.BaseController
   {

      #region Contants
      internal class Constants
      {
         // internal const string WARNING_TEXT_DUPLICITY = "MSG_CATEGORIES_TEXT_DUPLICITY";
      }
      #endregion

   }

}
