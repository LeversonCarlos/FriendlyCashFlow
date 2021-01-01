using Microsoft.Extensions.Configuration;

namespace example
{

   public class MongoSettings
   {
      public string ConnStr { get; set; }
      public string Database { get; set; }
   }

   internal static class MongoSettingsExtention
   {
      public static MongoSettings GetMongoSettings(this IConfiguration configuration) =>
         configuration
            .GetSection("Mongo")
            .Get<MongoSettings>();
   }

}
