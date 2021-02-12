using System.Reflection;

namespace Elesse.Shared
{
   partial class Translations
   {
      public static string GetVersion()
      {
         var version = Assembly.GetEntryAssembly().GetName().Version;
         var versionText = $"{version.Major}.{version.Minor}.{version.Build}";
         return versionText;
      }
   }
}
