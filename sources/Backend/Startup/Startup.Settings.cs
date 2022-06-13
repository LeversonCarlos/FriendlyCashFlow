using Lewio.CashFlow.Models;

namespace Lewio.CashFlow;

partial class StartupExtensions
{

   public static IServiceCollection ConfigureSettings(this IServiceCollection serviceCollection, IConfiguration configuration)
   {
      var settingsSection = configuration.GetSection("Settings");

      serviceCollection
         .Configure<Settings>(settingsSection);

      var settings = settingsSection.Get<Settings>();
      serviceCollection
         .AddSingleton(settings);

      return serviceCollection;
   }

}
