using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Elesse.Patterns;

namespace Elesse.Transfers
{

   public static class StartupExtentions
   {

      public static IMvcBuilder AddTransferService(this IMvcBuilder mvcBuilder, IConfiguration configs)
      {
         var services =
            mvcBuilder.Services;
         services
            .AddScoped<ITransferRepository, TransferRepository>()
            .AddScoped<ITransferService, TransferService>();
         mvcBuilder
            .AddPatternService(configs)
            .AddApplicationPart(Assembly.GetAssembly(typeof(TransferController)));
         return mvcBuilder;
      }

   }

}
