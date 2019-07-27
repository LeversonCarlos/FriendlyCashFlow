#region Using
using System.Web;
using Microsoft.Owin.Security;
#endregion

namespace FriendCash.Web.Auth
{  
   public partial class AuthController : Controllers.Base
   {

      #region Authentication
      IAuthenticationManager Authentication
      {
         get { return HttpContext.GetOwinContext().Authentication; }
      }
      #endregion

   }
}