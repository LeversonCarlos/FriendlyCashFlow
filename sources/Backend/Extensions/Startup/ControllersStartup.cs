namespace Lewio.CashFlow;

public static class ControllersStartupExtension
{
   const string DEVHOST = "localhost";

   public static IServiceCollection AddControllersServices(this IServiceCollection serviceCollection, IConfiguration configuration)
   {

      serviceCollection
         .AddCors(options =>
         {
            options.AddPolicy(DEVHOST, policy =>
            {
               policy
                  .AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
               // policy.WithOrigins("http://localhost:4200");
            });
         });

      serviceCollection
         .AddControllers()
         .AddJsonOptions(options =>
         {
            options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.PropertyNamingPolicy = null; // to use PascalCase
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            options.JsonSerializerOptions.WriteIndented = false;
         });

      return serviceCollection;
   }

   public static WebApplication AddControllersServices(this WebApplication app)
   {
      if (app.Environment.IsDevelopment())
         app.UseCors(DEVHOST);

      app
         .UseRouting()
         .UseEndpoints(endPoints =>
         {
            endPoints.MapControllers();
         });

      return app;
   }

}
