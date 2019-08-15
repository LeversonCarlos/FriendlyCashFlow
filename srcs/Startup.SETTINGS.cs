using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FriendlyCashFlow
{
   partial class Startup
   {

      private void AddSettingsServices(IServiceCollection services)
      {
         var appSettings = this.GetAppSettings(services);
         services.AddSingleton<Helpers.Mail>();
         services.AddSingleton<Helpers.Crypt>();
      }


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
      public AppSettingsPasswords Passwords { get; set; }
      public AppSettingsMail Mail { get; set; }
   }

   public class AppSettingsPasswords
   {
      public string PasswordSalt { get; set; }
      public bool RequireUpperCases { get; set; }
      public bool RequireLowerCases { get; set; }
      public bool RequireNumbers { get; set; }
      public bool RequireSymbols { get; set; }
      public short MinimumSize { get; set; }
   }

   public class AppSettingsMail
   {
      public string SmtpHost { get; set; }
      public short SmtpPort { get; set; }
      public string FromAddress { get; set; }
      public string FromName { get; set; }
      public string FromPassword { get; set; }
   }

}
