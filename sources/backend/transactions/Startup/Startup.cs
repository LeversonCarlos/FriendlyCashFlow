using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elesse.Transactions
{

   public static class StartupExtentions
   {

      public static IMvcBuilder AddTransactionService(this IMvcBuilder mvcBuilder, IConfiguration configs)
      {
         var services =
            mvcBuilder.Services;
         services
            // .AddScoped<ITransactionRepository, TransactionRepository>()
            .AddScoped<ITransactionService, TransactionService>();
         mvcBuilder
            .AddApplicationPart(Assembly.GetAssembly(typeof(TransactionController)));
         return mvcBuilder;
      }

   }

}
