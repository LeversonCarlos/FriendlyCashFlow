using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FriendlyCashFlow
{
   partial class Startup
   {

      private AppSettings GetAppSettings(IServiceCollection services)
      {
         var appSettingsSection = this.Configuration.GetSection("AppSettings");
         services.Configure<AppSettings>(appSettingsSection);
         var appSettings = appSettingsSection.Get<AppSettings>();
         return appSettings;
      }

   }

   public class AppSettings
   {
      // public AppSettings() { this.CustomValues = new Dictionary<string, string>(); }
      // public Dictionary<string, string> CustomValues { get; set; }
      public string ConnStr { get; set; }
      public string PasswordSalt { get; set; }
   }

}
