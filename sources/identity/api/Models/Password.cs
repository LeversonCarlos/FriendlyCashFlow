using System;

namespace FriendlyCashFlow.Identity
{
   internal struct Password
   {

      public Password(string value)
      {
         if (string.IsNullOrEmpty(value) || value.Length < 5)
            throw new ArgumentException(WARNING_IDENTITY_INVALID_PASSWORD_PARAMETER);
         _Value = value;
      }
      internal const string WARNING_IDENTITY_INVALID_PASSWORD_PARAMETER = "WARNING_IDENTITY_INVALID_PASSWORD_PARAMETER";

      readonly string _Value;

      public static implicit operator string(Password value) =>
         value._Value;

      public static implicit operator Password(string value) =>
         new Password(value);

   }
}