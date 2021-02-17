using Elesse.Shared;
using Elesse.Accounts;
using Elesse.Identity.Helpers;
using Elesse.Categories;
using Elesse.Entries;
using Elesse.Transfers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;


namespace Elesse.FriendlyCashFlow
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
            .AddSingleton(sp => _Configuration.GetMongoSettings())
            .AddSingleton<IMongoClient>(sp => new MongoClient(sp.GetService<MongoSettings>().ConnStr))
            .AddSingleton<IMongoDatabase>(sp => sp.GetService<IMongoClient>().GetDatabase(_Configuration.GetMongoSettings().Database))
            .AddCors(options => options.AddPolicy("FrontendPolicy", policy =>
            {
               policy
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .WithOrigins(_Configuration.GetFrontendSettings().Url);
            }))
            .AddControllers()
            .AddJsonOptions(options =>
            {
               options.JsonSerializerOptions.IgnoreNullValues = true;
               options.JsonSerializerOptions.PropertyNamingPolicy = null; // to use PascalCase
               options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            })
            .AddInsightsService(_Configuration)
            .AddIdentityService(_Configuration)
            .AddSharedService(_Configuration)
            .AddAccountService(_Configuration)
            .AddCategoryService(_Configuration)
            .AddEntryService(_Configuration)
            .AddTransferService(_Configuration);
      }

      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }

         app.UseCors("FrontendPolicy");
         app.UseRouting();
         app.UseAuthentication();
         app.UseAuthorization();

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
