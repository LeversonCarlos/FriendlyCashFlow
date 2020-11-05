using Elesse.Identity.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace example
{
   public class Startup
   {

      readonly IConfiguration _Configuration;
      public Startup(IConfiguration configuration)
      {
         _Configuration = configuration;
      }

      public void ConfigureServices(IServiceCollection services)
      {
         services
            .AddSingleton(sp => _Configuration.GetSection("Mongo").Get<MongoSettings>())
            .AddSingleton<IMongoClient>(sp => new MongoClient(sp.GetService<MongoSettings>().ConnStr))
            .AddSingleton<IMongoDatabase>(sp => sp.GetService<IMongoClient>().GetDatabase(sp.GetService<MongoSettings>().Database))
            .AddControllers()
            .AddJsonOptions(options =>
            {
               options.JsonSerializerOptions.IgnoreNullValues = true;
               options.JsonSerializerOptions.PropertyNamingPolicy = null; // to use PascalCase
               options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            })
            .AddIdentityService(_Configuration);
      }

      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }

         app.UseRouting();

         app.UseEndpoints(endpoints =>
         {
            endpoints.MapControllers();
         });

         /*
         app.UseEndpoints(endpoints =>
         {
            endpoints.MapGet("/", async context =>
               {
                  var sp = app.ApplicationServices.CreateScope().ServiceProvider;
                  var identityService = sp.GetService<IIdentityService>();

                  var userName = $"{System.Guid.NewGuid().ToString().Replace("-", "")}@xpto.com";
                  var result = await identityService.RegisterAsync(new RegisterVM { UserName = userName, Password = "password" });

                  if (result is OkResult)
                  {
                     await context.Response.WriteAsync($"User {userName} Registered!");
                     return;
                  }

                  var badRequest = result as BadRequestObjectResult;
                  var badRequestContent = badRequest.Value as string[];
                  context.Response.StatusCode = badRequest.StatusCode.Value;
                  await context.Response.WriteAsync(string.Join(", ", badRequestContent));

               });
         });
         */

      }
   }
}