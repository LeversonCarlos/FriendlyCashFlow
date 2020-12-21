using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Identity.Tests
{
   internal class ControllerHelper
   {

      public static ControllerContext GetControllerContext(string userID) =>
         GetControllerContext(new System.Security.Claims.ClaimsIdentity(new System.Security.Principal.GenericIdentity(userID, "Login")));

      public static ControllerContext GetControllerContext(System.Security.Claims.ClaimsIdentity claimsIdentity) =>
         GetControllerContext(new System.Security.Claims.ClaimsPrincipal(claimsIdentity));

      public static ControllerContext GetControllerContext(System.Security.Claims.ClaimsPrincipal claimsPrincipal) =>
         new ControllerContext
         {
            HttpContext = new DefaultHttpContext
            {
               User = claimsPrincipal
            }
         };

   }
}