using System.Collections.Generic;
using Xunit;

namespace Elesse.Identity.Tests
{
   partial class IdentityServiceTests
   {

      [Theory]
      [MemberData(nameof(GetHashedText_WithSuppliedParameters_MustResultExpectedValue_Data))]
      internal void GetHashedText_WithSuppliedParameters_MustResultExpectedValue(string password, string salt, string expected)
      {
         var settings = new IdentitySettings { PasswordSalt = salt };
         var result = password.GetHashedText(settings.PasswordSalt);

         Assert.Equal(expected, result);
      }
      public static IEnumerable<object[]> GetHashedText_WithSuppliedParameters_MustResultExpectedValue_Data() =>
         new[] {
            new object[] { "password", "", "X03MO1qnZdYdgyfeuILPmQ==" },
            new object[] { "password", "a1b2c3d4", "tCxLxOR+/6yEqVTkg4SZCg==" },
            new object[] { "password", "q1a2z3x4s5w6e7d8c9v0f!r@t#g$b%n¨h&y*u(j)m-k=i[o~l;p", "H1DpoUAoJeHl1IxsT0j06w==" }
         };

   }
}
