using System;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace FriendlyCashFlow.Identity.Tests
{
   partial class IdentityServiceTests
   {

      [Theory]
      [MemberData(nameof(HashPassword_WithSuppliedParameters_MustResultExpectedValue_Data))]
      internal void HashPassword_WithSuppliedParameters_MustResultExpectedValue(string password, string salt, string expected)
      {
         var settings = new PasswordSettings { PasswordSalt = salt };
         var identityService = new IdentityService(null, settings);

         var result = identityService.HashPassword(password);

         Assert.Equal(expected, result);
      }
      public static IEnumerable<object[]> HashPassword_WithSuppliedParameters_MustResultExpectedValue_Data() =>
         new[] {
            new object[] { "password", "", "X03MO1qnZdYdgyfeuILPmQ==" }, 
            new object[] { "password", "a1b2c3d4", "tCxLxOR+/6yEqVTkg4SZCg==" },
            new object[] { "password", "q1a2z3x4s5w6e7d8c9v0f!r@t#g$b%n¨h&y*u(j)m-k=i[o~l;p", "H1DpoUAoJeHl1IxsT0j06w==" }
         };

   }
}
