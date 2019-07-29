using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace FriendlyCashFlow
{
   partial class Startup
   {

      private void AddMvcServices(IServiceCollection services)
      {
         services.AddMvc()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
      }

      private void UseMvcServices(IApplicationBuilder app, IHostingEnvironment env)
      {
         app.UseMvc(routes =>
         {
            routes.MapRoute(
               name: "default",
               template: "{controller}/{action=Index}/{id?}");
         });
      }

   }
}
