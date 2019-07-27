#region Using
using System;
using System.Web.Http;
#endregion

namespace FriendCash.Auth
{

   [RoutePrefix("api/auth")]
   public partial class AuthController : Service.Base.BaseController
   {
      public AuthController() {
         this.authContext = new Model.dbContext();
      }

      private Model.dbContext authContext { get; set; }

      public new void Dispose() 
      {
         base.Dispose();
         this.authContext.Dispose();
      }

   }

}
