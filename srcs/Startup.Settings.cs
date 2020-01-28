using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FriendlyCashFlow
{
   partial class Startup
   {

      private AppSettings AppSettings { get; set; }
      private void AddSettings(IServiceCollection services)
      {

         var appSettingsSection = this.Configuration.GetSection("AppSettings");
         services.Configure<AppSettings>(appSettingsSection);
         this.AppSettings = appSettingsSection.Get<AppSettings>();

         services.AddSingleton<Helpers.Mail>();
         services.AddSingleton<Helpers.Crypt>();
         services.AddSingleton<Helpers.PasswordStrengthService>();
      }

      private void UseSettings(IApplicationBuilder app, IWebHostEnvironment env)
      {
         if (env.IsDevelopment())
         { app.UseDeveloperExceptionPage(); }
         else
         {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days.
            // You may want to change this for production scenarios,
            // see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
         }

         app.UseHttpsRedirection();

         app.UseStaticFiles();
         if (!env.IsDevelopment())
         { app.UseSpaStaticFiles(); }
      }

   }

   public class AppSettings
   {
      // public AppSettings() { this.CustomValues = new Dictionary<string, string>(); }
      // public Dictionary<string, string> CustomValues { get; set; }
      public string BaseHost { get; set; }
      public string ConnStr { get; set; }
      public AppSettingsPasswords Passwords { get; set; }
      public AppSettingsToken Token { get; set; }
      public AppSettingsMail Mail { get; set; }
      public AppSettingsAppInsights AppInsights { get; set; }
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

   public class AppSettingsToken
   {
      public string Secret { get; set; }
      public string Issuer { get; set; }
      public string Audience { get; set; }
      public int AccessExpirationInSeconds { get; set; }
      public int RefreshExpirationInSeconds { get; set; }
   }

   public class AppSettingsAppInsights : Helpers.AppInsights.Settings
   {
      public bool Activated { get; set; }
   }

}
