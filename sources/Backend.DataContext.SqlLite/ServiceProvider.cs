using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace Lewio.CashFlow;

public static class ServiceCollectionExtension
{

   public static IServiceCollection AddDataContext<T>(this IServiceCollection serviceCollection, string connectionString)
      where T: DbContext
   {
      serviceCollection
         .AddDbContext<T>(options => options.UseSqlite(connectionString));
      return serviceCollection;
   }

}
