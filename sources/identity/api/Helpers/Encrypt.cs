using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace FriendlyCashFlow.Identity
{

   partial class IdentityService
   {

      internal string HashPassword(string password)
      {

         // INCOME BYTES
         var saltStack = new Queue(Encoding.UTF8.GetBytes(_Settings.PasswordSalt ?? ""));
         var valueStack = new Queue(Encoding.UTF8.GetBytes(password));

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
         using (var hasher = MD5.Create())
         { computedHash = hasher.ComputeHash(mergedBytes); }

         // RESULT
         var result = Convert.ToBase64String(computedHash);
         return result;

      }

   }

}
