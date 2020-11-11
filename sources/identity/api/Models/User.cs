using System.Linq;
using System.Security.Claims;

namespace Elesse.Identity
{
   internal class User : IUser
   {

      public string UserID { get; set; }
      public string UserName { get; set; }

      public string[] Roles { get; set; }

      internal void ApplyIdentity(ClaimsIdentity claimsIdentity)
      {
         UserID = claimsIdentity.Name;
         UserName = claimsIdentity.Claims
            .Where(claim => claim.Type == ClaimTypes.NameIdentifier)
            .Select(claim => claim.Value)
            .FirstOrDefault();
         Roles = claimsIdentity.Claims
            .Where(claim => claim.Type == ClaimTypes.Role)
            .Select(claim => claim.Value)
            .ToArray();
      }

   }
}
