using FriendlyCashFlow.Identity.Helpers;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Xunit;

namespace FriendlyCashFlow.Identity.Tests
{
   public class TokenTests
   {

      [Fact]
      internal void GetSecurityKey_WithNullParameter_MustThrowException()
      {
         var value = Assert.Throws<ArgumentNullException>(() => Token.GetSecurityKey(null));

         Assert.NotNull(value);
         Assert.Contains("The SecuritySecret property of the TokenSettings is required to build a SecurityKey", value.Message);
      }

      [Fact]
      internal void GetSecurityKey_WithEmptySecuritySecret_MustThrowException()
      {
         var settings = new TokenSettings { SecuritySecret = "" };

         var value = Assert.Throws<ArgumentNullException>(() => Token.GetSecurityKey(settings));

         Assert.NotNull(value);
         Assert.Contains("The SecuritySecret property of the TokenSettings is required to build a SecurityKey", value.Message);
      }

      [Theory]
      [InlineData("abc1234")]
      [InlineData("1")]
      [InlineData("1234567890")]
      internal void GetSecurityKey_WithSpecifiedParameter_MustResultExpected(string securitySecret)
      {
         var settings = new TokenSettings { SecuritySecret = securitySecret };
         var expected = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(securitySecret));

         var value = Token.GetSecurityKey(settings);

         Assert.NotNull(value);
         Assert.Equal(expected.Key, value.Key);
      }

      [Fact]
      internal void GetTokenDescriptor_WithNullIdentity_MustThrowException()
      {
         ClaimsIdentity identity = null;
         TokenSettings settings = new TokenSettings { };

         var value = Assert.Throws<ArgumentNullException>(() => Token.GetTokenDescriptor(identity, settings));

         Assert.NotNull(value);
         Assert.Contains("The Identity parameter is required for the GetTokenDescriptor function on the Token class", value.Message);
      }

      [Fact]
      internal void GetTokenDescriptor_WithNullSettings_MustThrowException()
      {
         ClaimsIdentity identity = new ClaimsIdentity { };
         TokenSettings settings = null;

         var value = Assert.Throws<ArgumentNullException>(() => Token.GetTokenDescriptor(identity, settings));

         Assert.NotNull(value);
         Assert.Contains("The AccessExpirationInSeconds property on the Settings parameter is required for the GetTokenDescriptor function on the Token class", value.Message);
      }

      [Theory]
      [InlineData("issuer", "audience", "userID", "userName", 1)]
      [InlineData("my-issuer-name", "my-audience-name", "b36b857e-3748-4b2f-9f4d-6bacb8ffec51", "my-lengthy-username", 77)]
      [InlineData("i.s.s.u.e.r", "a-u-d-i-e-n-c-e", "user-ID", "user-name", int.MaxValue)]
      internal void GetTokenDescriptor_WithEspeciedParameters_MustResultExpected(string issuer, string audience, string userID, string userName, int accessExpirationInSeconds)
      {
         ClaimsIdentity identity = new ClaimsIdentity(
            new GenericIdentity(userID, "Login"),
            new List<Claim>{
               new Claim(ClaimTypes.NameIdentifier, userName)
            });
         TokenSettings settings = new TokenSettings
         {
            SecuritySecret = "abc1234",
            AccessExpirationInSeconds = accessExpirationInSeconds,
            Audience = audience,
            Issuer = issuer
         };

         var value = Token.GetTokenDescriptor(identity, settings);

         Assert.NotNull(value);
         Assert.Equal(issuer, value.Issuer);
         Assert.Equal(audience, value.Audience);
         Assert.Equal(userID, value.Subject.Name);
         Assert.Equal(userName, value.Subject.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault());
         Assert.Equal(accessExpirationInSeconds, (int)value.Expires.Value.Subtract(value.NotBefore.Value).TotalSeconds);
      }

   }
}
