using Lewio.CashFlow.Settings;
namespace Lewio.CashFlow;

public static class SettingsStartupExtension
{
   public static IServiceCollection AddSettingsServices(this IServiceCollection serviceCollection, IConfiguration configuration)
   {

      var settingsSection = configuration
         .GetSection("Settings");
      var settings = settingsSection
         .Get<SettingsVM>();

      serviceCollection
         .AddSingleton(settings);

      return serviceCollection;
   }
}