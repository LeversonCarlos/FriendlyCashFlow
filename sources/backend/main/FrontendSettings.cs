using Microsoft.Extensions.Configuration;

namespace example
{

   public class FrontendSettings
   {
      public string Url { get; set; }
   }

   internal static class FrontendSettingsExtention
   {
      public static FrontendSettings GetFrontendSettings(this IConfiguration configuration) =>
         configuration
            .GetSection("Frontend")
            .Get<FrontendSettings>();
   }

}
