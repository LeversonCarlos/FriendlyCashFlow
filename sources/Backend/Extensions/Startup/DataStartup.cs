using Microsoft.EntityFrameworkCore;

public static class DataStartupExtension
{
   public static IServiceCollection AddDataServices(this IServiceCollection serviceCollection, IConfiguration configuration)
   {

      var connectionString = configuration.GetConnectionString("DataConnection") ?? throw new InvalidOperationException("Connection string 'DataConnection' wasnt found on appsettings");

      serviceCollection
         .AddDbContext<DataContext>(options => options.UseSqlite(connectionString));
      // serviceCollection
      //    .AddDatabaseDeveloperPageExceptionFilter();

      return serviceCollection;
   }
}
