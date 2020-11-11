using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using Xunit;

namespace Elesse.Identity.Tests
{
   public class UserTests
   {


      [Fact]
      public void ApplyIdentity_WithValidParameters_MustSetClassProperties()
      {
         var userID = "my-user-dD";
         var userName = "my-user-name";
         var roleID = "my-role-id";

         var claimsIdentity = new ClaimsIdentity(
             new GenericIdentity(userID, "Login"),
             new List<Claim> {
               new Claim(ClaimTypes.NameIdentifier, userName),
               new Claim(ClaimTypes.Role, roleID)
             }.ToArray()
          );

         var user = new User();
         user.ApplyIdentity(claimsIdentity);

         Assert.Equal(userID, user.UserID);
         Assert.Equal(userName, user.UserName);
         Assert.Equal(new string[] { roleID }, user.Roles);
      }

   }
}