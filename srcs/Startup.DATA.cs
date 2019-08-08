using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FriendlyCashFlow
{
   partial class Startup
   {

      private void AddDataServices(IServiceCollection services)
      {
         var appSettings = this.GetAppSettings(services);
         services.AddDbContext<API.Base.dbContext>(x =>
            x.UseSqlServer(appSettings.ConnStr, opt =>
            {
               opt.MigrationsHistoryTable("v6_MigrationsHistory");
            }));
         // services.AddDbContext<API.Shared.dbContext>(x => x.UseInMemoryDatabase("FriendlyCashFlowDb"));
      }

   }
}
