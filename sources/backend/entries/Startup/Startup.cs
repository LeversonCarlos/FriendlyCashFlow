using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Elesse.Patterns;

namespace Elesse.Entries
{

   public static class StartupExtentions
   {

      public static IMvcBuilder AddEntryService(this IMvcBuilder mvcBuilder, IConfiguration configs)
      {
         var services =
            mvcBuilder.Services;
         services
            .AddScoped<IEntryRepository, EntryRepository>()
            .AddScoped<IEntryService, EntryService>();
         mvcBuilder
            .AddPatternService(configs)
            .AddApplicationPart(Assembly.GetAssembly(typeof(EntryController)));
         return mvcBuilder;
      }

   }

}
