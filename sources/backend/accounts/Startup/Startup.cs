using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elesse.Accounts
{

   public static class StartupExtentions
   {

      public static IMvcBuilder AddAccountService(this IMvcBuilder mvcBuilder, IConfiguration configs)
      {
         var services =
            mvcBuilder.Services;
         services
            .AddScoped<IAccountRepository, AccountRepository>()
            .AddScoped<IAccountService, AccountService>();
         mvcBuilder
            .AddApplicationPart(Assembly.GetAssembly(typeof(AccountController)));
         return mvcBuilder;
      }

   }

}
