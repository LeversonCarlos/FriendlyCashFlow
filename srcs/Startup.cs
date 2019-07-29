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
         this.AddMvcServices(services);
         this.AddSpaServices(services);
      }

      public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
         app.UseSpaStaticFiles();
         this.UseMvcServices(app, env);
         this.UseSpaServices(app, env);
      }
   }
}
