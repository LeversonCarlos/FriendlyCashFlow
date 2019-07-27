#region Using
using System.Security.Cryptography;
using System.Text;
#endregion

namespace FriendCash.Model.Base
{
   public class Hash
   {
      public static string Execute(string Value)
      {
         try
         {

            // COMPUTE HASH
            HashAlgorithm algorithm = MD5.Create();  /* OR SHA1.Create() */
            var aHash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(Value));

            // BUILDER ('X2' PRINTS THE STRING AS TWO UPPERCASE HEXADECIMAL CHARACTERS)
            StringBuilder sb = new StringBuilder();
            foreach (byte b in aHash)
               sb.Append(b.ToString("X2"));

            // RETURN
            return sb.ToString();
         }
         catch { throw; }
      }
   }
}
