#region Using
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
#endregion

namespace FriendCash.Auth.Model
{
   public class bindUser : IdentityUser
   {

      #region Properties

      [MaxLength(255)]
      public string FullName { get; set; }

      [Required]
      public DateTime JoinDate { get; set; }

      public DateTime ExpirationDate { get; set; }

      #endregion

      #region GenerateUserIdentityAsync
      public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<bindUser> manager, string authenticationType)
      {
         var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
         await this.ReviewClaims(userIdentity);
         return userIdentity;
      }
      #endregion

      #region ReviewClaims

      public async Task ReviewClaims(ClaimsIdentity userIdentity)
      {
         try
         {
            await this.ReviewClaims_SignatureExpiration(userIdentity);
            await this.ReviewClaims_ActiveRole(userIdentity);
         }
         catch (Exception ex) { throw ex; }
      }

      private async Task ReviewClaims_SignatureExpiration(ClaimsIdentity userIdentity)
      {
         try
         {

            // REMOVE CURRENT EXPIRATION
            var expirationClaim = await Task.FromResult(userIdentity.Claims.FirstOrDefault(x => x.Type == "SignatureExpiration"));
            if (expirationClaim != null)
            { var result = userIdentity.TryRemoveClaim(expirationClaim); }

            // ADD EXPIRATION
            var expirationDate = this.ExpirationDate.ToString("yyyy-MM-dd HH:mm:ss");
            expirationClaim = new Claim("SignatureExpiration", expirationDate);
            userIdentity.AddClaim(expirationClaim);
            
         }
         catch (Exception ex) { throw ex; }
      }

      private async Task ReviewClaims_ActiveRole(ClaimsIdentity userIdentity)
      {
         try
         {

            // ACTIVE
            var activeUser = true;
            if (!this.EmailConfirmed) { activeUser = false; }
            if (this.ExpirationDate <= DateTime.UtcNow) { activeUser = false; }
            Claim claim = null;

            // REMOVE ACTIVE ROLE
            claim = await Task.FromResult(userIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role && x.Value == "ActiveUser"));
            if (claim != null) { var result = userIdentity.TryRemoveClaim(claim); }

            //// REMOVE ACTIVE FLAG
            //claim = await Task.FromResult(userIdentity.Claims.FirstOrDefault(x => x.Type == "ActiveUser"));
            //if (claim != null) { var result = userIdentity.TryRemoveClaim(claim); }

            // ADD ACTIVE ROLE
            if (activeUser)
            {
               claim = await Task.FromResult(userIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role && x.Value == "ActiveUser"));
               if (claim == null)
               {
                  claim = new Claim(ClaimTypes.Role, "ActiveUser");                  
                  userIdentity.AddClaim(claim);
               }
            }

            //// ADD ACTIVE FLAG
            //claim = new Claim("ActiveUser", (activeUser ? "1" : "0"));
            //userIdentity.AddClaim(claim);

         }
         catch (Exception ex) { throw ex; }
      }

      #endregion

   }
}