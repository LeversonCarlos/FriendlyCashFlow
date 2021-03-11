using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elesse.Recurrences
{

   public static class StartupExtentions
   {

      public static IMvcBuilder AddRecurrenceService(this IMvcBuilder mvcBuilder, IConfiguration configs)
      {
         var services =
            mvcBuilder.Services;
         services
            .AddScoped<IRecurrenceRepository, RecurrenceRepository>()
            .AddScoped<IRecurrenceService, RecurrenceService>();
         try { MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<RecurrenceEntity>(); } catch { }
         return mvcBuilder;
      }

   }

}
