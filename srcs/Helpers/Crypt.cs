using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FriendlyCashFlow.Helpers
{
   internal class Crypt
   {

      private readonly IOptions<AppSettings> _appSettings;
      public Crypt([FromServices] IOptions<AppSettings> appSettings) { this._appSettings = appSettings; }

      public string Encrypt(string value)
      {

         // INCOME BYTES
         var saltStack = new Queue(Encoding.ASCII.GetBytes(this._appSettings.Value.Passwords.PasswordSalt));
         var valueStack = new Queue(Encoding.ASCII.GetBytes(value));

         // MERGE BYTES
         byte[] mergedBytes = new byte[valueStack.Count + saltStack.Count];
         int mergedIndex = 0; int mergedLength = mergedBytes.Length;
         while (mergedIndex < mergedLength)
         {
            if (saltStack.Count > 0)
            { mergedBytes[mergedIndex++] = (byte)saltStack.Dequeue(); }
            if (valueStack.Count > 0)
            { mergedBytes[mergedIndex++] = (byte)valueStack.Dequeue(); }
         }

         // HASH
         byte[] computedHash;
         using (MD5 hasher = MD5.Create())
         { computedHash = hasher.ComputeHash(mergedBytes); }

         // RESULT
         return Convert.ToBase64String(computedHash);
      }

   }
}
