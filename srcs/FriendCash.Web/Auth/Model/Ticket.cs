#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
#endregion

namespace FriendCash.Web.Auth.Model
{

   public interface iAuthPrincipal : IPrincipal
   {
      string ID { get; set; }
      string UserName { get; set; }
      string FullName { get; set; }
      string Email { get; set; }
      string Authorization { get; set; }
   }

   public class AuthPrincipal : iAuthPrincipal
   {
      public AuthPrincipal(string email) { this.Email = email; this.Identity = new GenericIdentity(email); }
      public bool IsInRole(string role) { return false; }
      public string ID { get; set; }
      public string UserName { get; set; }
      public string FullName { get; set; }
      public string Email { get; set; }
      public string Authorization { get; set; }
      public IIdentity Identity { get; private set; }
   }

   public class AuthTicket
   {
      public string ID { get; set; }
      public string UserName { get; set; }
      public string FullName { get; set; }
      public string Email { get; set; }
      public string Authorization { get; set; }
   }

}