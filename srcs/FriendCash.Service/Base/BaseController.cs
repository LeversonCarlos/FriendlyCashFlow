#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
#endregion

namespace FriendCash.Service.Base
{
   public partial class BaseController : ApiController
   {

      #region New
      public BaseController()
      {
         this.DataContext = new dbContext();
      }
      #endregion

      #region DataContext
      internal dbContext DataContext { get; set; }
      #endregion

      #region GetWebHostAddress
      internal string GetWebHostAddress() { return System.Configuration.ConfigurationManager.AppSettings["web:HostAddress"]; }
      #endregion

   }
}