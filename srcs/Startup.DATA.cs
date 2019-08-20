using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FriendlyCashFlow
{
   partial class Startup
   {

      private void AddDataServices(IServiceCollection services)
      {

         // DATA CONTEXT
         var appSettings = this.GetAppSettings(services);
         services.AddDbContext<API.Base.dbContext>(x =>
            x.UseSqlServer(appSettings.ConnStr, opt =>
            {
               opt.MigrationsHistoryTable("v6_MigrationsHistory");
            }));

         // CONFIGURE INJECTION FOR DATA SERVICES
         services.AddScoped<API.Accounts.AccountsService>();

      }

   }
}
