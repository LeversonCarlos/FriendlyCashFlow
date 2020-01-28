using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FriendlyCashFlow
{
   public partial class Startup
   {

      public IConfiguration Configuration { get; }
      public Startup(IConfiguration configuration)
      {
         this.Configuration = configuration;
      }

      public void ConfigureServices(IServiceCollection services)
      {
         this.AddSettings(services);
         this.AddLocalization(services);
         this.AddServices(services);
         this.AddAuthentication(services);
         this.AddControllers(services);
      }

      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         this.UseSettings(app, env);
         this.UseLocalization(app, env);
         this.UseControllers(app, env);
      }
   }
}
