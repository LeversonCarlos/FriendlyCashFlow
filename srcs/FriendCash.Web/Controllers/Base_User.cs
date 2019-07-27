#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
#endregion

namespace FriendCash.Web.Controllers
{
   partial class Base
   {

      public UserTicket UserTicket
      {
         get
         {
            if (ViewBag.UserTicket == null) { ViewBag.UserTicket = new UserTicket(this.User); }
            return ViewBag.UserTicket;
         }
      }

   }

   #region viewUserTicket
   public class UserTicket
   {

      #region New
      public UserTicket(IPrincipal userPrincipal)
      {

         // BASIC
         var nowTime = DateTime.Now;
         this.UserName = userPrincipal.Identity.Name;
         this.IsAuthenticated = userPrincipal.Identity.IsAuthenticated;
         this.AuthenticationType = userPrincipal.Identity.AuthenticationType;

         // CLAIMS
         if (userPrincipal.GetType().IsAssignableFrom(typeof(ClaimsPrincipal)))
         {
            var oClaimsPrincipal = (ClaimsPrincipal)userPrincipal;
            this.ID = oClaimsPrincipal.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => x.Value).FirstOrDefault();
            this.UserName = oClaimsPrincipal.Claims.Where(x => x.Type == ClaimTypes.Name).Select(x => x.Value).FirstOrDefault();
            this.Email = oClaimsPrincipal.Claims.Where(x => x.Type == ClaimTypes.Email).Select(x => x.Value).FirstOrDefault();
            this.IsPersistent = (oClaimsPrincipal.Claims.Where(x => x.Type == ClaimTypes.IsPersistent).Select(x => x.Value).FirstOrDefault() == "1");
            this.AuthorizationType = oClaimsPrincipal.Claims.Where(x => x.Type == ClaimTypes.AuthenticationMethod).Select(x => x.Value).FirstOrDefault();
            this.Authorization = oClaimsPrincipal.Claims.Where(x => x.Type == ClaimTypes.Authentication).Select(x => x.Value).FirstOrDefault();
            this.RefreshToken = oClaimsPrincipal.Claims.Where(x => x.Type == "RefreshToken").Select(x => x.Value).FirstOrDefault();
            this.RefreshExpiration = DateTime.Parse(oClaimsPrincipal.Claims.Where(x => x.Type == "RefreshExpiration").Select(x => x.Value).FirstOrDefault());
            // this.SignatureExpiration = DateTime.Parse(oClaimsPrincipal.Claims.Where(x => x.Type == "SignatureExpiration").Select(x => x.Value).FirstOrDefault());
            this.FullName = oClaimsPrincipal.Claims.Where(x => x.Type == ClaimTypes.GivenName).Select(x => x.Value).FirstOrDefault();

            // ACCESS EXPIRATION
            this.AccessExpiration = DateTime.Parse(oClaimsPrincipal.Claims.Where(x => x.Type == ClaimTypes.Expiration).Select(x => x.Value).FirstOrDefault());
            if (this.AccessExpiration > nowTime)
            {
               this.AccessExpirationSeconds = (long)this.AccessExpiration.Subtract(nowTime).TotalSeconds;
               this.AccessExpirationSeconds -= 60; 
            }
            if (this.AccessExpirationSeconds <= 0) { this.AccessExpirationSeconds = 1; }

            // ROLES
            var roleClaims = oClaimsPrincipal.Claims.Where(x => x.Type == "Roles").Select(x => x.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(roleClaims))
            {
               this.Roles = FriendCash.Model.Base.Json.Deserialize<List<string>>(roleClaims);
               this.IsActiveUser = (this.Roles.Count(x => x == "ActiveUser") != 0);
            }

         }
         if (string.IsNullOrEmpty(this.FullName)) { this.FullName = this.UserName; }

      }
      #endregion

      // public bool IsInRole(string role) { return false; }
      internal string ID { get; set; }
      public string UserName { get; set; }
      public string FullName { get; set; }
      public string Email { get; set; }
      public bool IsAuthenticated { get; set; }
      public bool IsPersistent { get; set; }
      internal string AuthenticationType { get; set; }
      internal string Authorization { get; set; }
      internal string AuthorizationType { get; set; }
      internal string RefreshToken { get; set; }
      internal List<string> Roles { get; set; }
      public bool IsActiveUser { get; set; }
      public DateTime RefreshExpiration { get; set; }
      public DateTime AccessExpiration { get; set; }
      public long AccessExpirationSeconds { get; set; }
      // public DateTime SignatureExpiration { get; set; }
   }
   #endregion

}