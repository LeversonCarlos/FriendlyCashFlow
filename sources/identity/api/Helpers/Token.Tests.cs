using System;
using FriendlyCashFlow.Identity.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Xunit;

namespace FriendlyCashFlow.Identity.Tests
{
   public class TokenTests
   {

      [Fact]
      internal void GetSecurityKey_WithNullParameter_MustThrowException()
      {
         var value = Assert.Throws<ArgumentException>(() => Token.GetSecurityKey(null));
         Assert.NotNull(value);
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

   }
}
