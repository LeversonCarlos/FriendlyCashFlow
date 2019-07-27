#region Using
using System;
using System.Web;
using System.Web.Security;
using FriendCash.Web.Auth.Model;
using FriendCash.Auth.Model;
#endregion

namespace FriendCash.Web.Auth
{
   public class Cookies
   {

      #region GetCookie
      internal static HttpCookie GetCookie(viewUser userResult, FriendCash.Web.Controllers.viewApiToken tokenResult)
      {
         try
         {

            // USER DATA
            var userData = new AuthTicket()
            {
               ID = userResult.ID,
               UserName = userResult.UserName,
               Email = userResult.Email,
               FullName = userResult.FullName,
               Authorization = tokenResult.token_type + " " + tokenResult.access_token
            };
            var userDataJson = Newtonsoft.Json.JsonConvert.SerializeObject(userData);

            // TICKET
            var authExpire = DateTime.UtcNow.AddSeconds(tokenResult.expires_in);
            var authTicket = new FormsAuthenticationTicket(1, userResult.Email, DateTime.UtcNow, authExpire, true, userDataJson);
            var authTicketCrypted = FormsAuthentication.Encrypt(authTicket);

            // RESULT
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, authTicketCrypted);
            authCookie.Expires = authExpire;
            return authCookie;

         }
         catch { throw; }
      }
      #endregion

      #region SetCookie
      internal static AuthPrincipal SetCookie(HttpCookie authCookie)
      {
         try
         {
            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            // USER DATA
            var userDataJson = authTicket.UserData;
            var userData = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthTicket>(userDataJson);

            // PRINCIPAL
            var userPrincipal = new AuthPrincipal(userData.Email);
            userPrincipal.ID = userData.ID;
            userPrincipal.UserName = userData.UserName;
            userPrincipal.FullName = userData.FullName;
            userPrincipal.Email = userData.Email;
            userPrincipal.Authorization = userData.Authorization;

            // RESULT
            return userPrincipal;
         }
         catch { throw; }
      }
      #endregion

   }
}