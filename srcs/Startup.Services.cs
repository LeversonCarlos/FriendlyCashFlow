using FriendlyCashFlow.Helpers.AppInsights;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FriendlyCashFlow
{
   partial class Startup
   {

      private void AddServices(IServiceCollection services)
      {

         // DATA CONTEXT
         services.AddDbContext<API.Base.dbContext>(x =>
            x.UseSqlServer(this.AppSettings.ConnStr, opt =>
            {
               opt.MigrationsHistoryTable("v6_MigrationsHistory");
            }));

         // CONFIGURE INJECTION FOR HELPERS
         services.AddScoped<API.Users.UsersService>();
         services.AddScoped<Helpers.User>();
         services.AddScoped<Helpers.DataReaderService>();

         // CONFIGURE INJECTION FOR DATA SERVICES
         services.AddScoped<API.Accounts.AccountsService>();
         services.AddScoped<API.Categories.CategoriesService>();
         services.AddScoped<API.Patterns.PatternsService>();
         services.AddScoped<API.Balances.BalancesService>();
         services.AddScoped<API.Recurrencies.RecurrenciesService>();
         services.AddScoped<API.Entries.EntriesService>();
         services.AddScoped<API.Transfers.TransfersService>();
         services.AddScoped<API.Dashboard.DashboardService>();

         // APPLICATION INSIGHTS
         if (this.AppSettings.AppInsights.Activated && !string.IsNullOrEmpty(this.AppSettings.AppInsights.InstrumentationKey))
         {
            services.AddApplicationInsights(this.AppSettings.AppInsights);
         }

      }

   }
}
