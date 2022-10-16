namespace Lewio.CashFlow;

public static class DataStartupExtension
{
   public static IServiceCollection AddDataServices(this IServiceCollection serviceCollection, IConfiguration configuration)
   {

      var connectionString = configuration.GetConnectionString("DataConnection") ?? throw new InvalidOperationException("Connection string 'DataConnection' wasnt found on appsettings");

      serviceCollection
         .AddDataContext<Common.DataContext>();
      // serviceCollection
      //    .AddDatabaseDeveloperPageExceptionFilter();

      serviceCollection
         .AddScoped<IMainRepository, MainRepository>();

      return serviceCollection;
   }
}
