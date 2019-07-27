#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
#endregion

namespace FriendCash.Service.Base
{
   partial class BaseController
   {

      #region UrlHelper
      private UrlHelper _UrlHelper;
      internal UrlHelper UrlHelper
      {
         get { return this._UrlHelper ?? new UrlHelper(this.Request); }
      }
      #endregion

   }
}