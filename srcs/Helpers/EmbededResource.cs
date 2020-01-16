using System;
using System.Reflection;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Helpers
{
   internal class EmbededResource
   {

      public static async Task<string> GetResourceContent(string resourcePath)
      {
         try
         {
            var resourceAssembly = typeof(FriendlyCashFlow.Program)
               .GetTypeInfo()
               .Assembly;
            using (var resourceStream = resourceAssembly.GetManifestResourceStream(resourcePath))
            {
               using (var resourceReader = new System.IO.StreamReader(resourceStream))
               {
                  return await resourceReader.ReadToEndAsync();
               }
            }
         }
         catch (Exception) { throw; }
      }

   }
}
