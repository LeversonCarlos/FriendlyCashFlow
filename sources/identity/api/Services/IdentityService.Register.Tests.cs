using System;

namespace FriendlyCashFlow.Identity.Tests
{
   public class IdentityServiceTests
   {

      [Fact]
      public void Register_WithInvalidParameters_MustThrowException()
      {
         var exception = Assert.Throws<ArgumentException>(() => new User(userID, userName, password));

         Assert.NotNull(exception);
         Assert.Equal(exceptionText, exception.Message);
      }

   }
}
