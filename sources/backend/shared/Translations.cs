namespace Elesse.Shared
{

   public partial class Translations
   {
      public string Version { get; private set; }
      public string Language { get; private set; }
   }

   public static Translations Create(Microsoft.AspNetCore.Http.HttpContext _HttpContext)
   {
      var translation = new Translations
      {
         Version = GetVersion(),
         Language = GetLanguageID(_HttpContext)
      };
      return translation;
   }

}
