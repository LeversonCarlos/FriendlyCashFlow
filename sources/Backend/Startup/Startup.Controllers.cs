namespace Lewio.CashFlow;

partial class StartupExtensions
{

   public static IServiceCollection ConfigureControllers(this IServiceCollection serviceCollection)
   {

      serviceCollection
         .AddControllers()
         .AddJsonOptions(options =>
         {
            options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.PropertyNamingPolicy = null; // to use PascalCase
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
         });

      return serviceCollection;
   }

}
