using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elesse.Shared
{

   public static class StartupExtentions
   {

      public static IMvcBuilder AddSharedService(this IMvcBuilder mvcBuilder, IConfiguration configs)
      {
         var services =
            mvcBuilder.Services;
         mvcBuilder
            .AddApplicationPart(Assembly.GetAssembly(typeof(SharedController)));
         return mvcBuilder;
      }

   }

}
