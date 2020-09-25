using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace FriendlyCashFlow.Identity
{
   internal static class EncryptExtention
   {

      internal static string GetHashedText(this string value, string salt)
      {

         // INCOME BYTES
         var saltStack = new Queue(Encoding.UTF8.GetBytes(salt ?? ""));
         var valueStack = new Queue(Encoding.UTF8.GetBytes(value));

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
