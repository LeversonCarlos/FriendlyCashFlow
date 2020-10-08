using FriendlyCashFlow.Identity.Helpers;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
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
         var value = Assert.Throws<ArgumentException>(() => Token.GetSecurityKey(settings));
         Assert.NotNull(value);
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

   }
}
