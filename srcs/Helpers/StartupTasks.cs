using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FriendlyCashFlow
{

   public interface IStartupTask
   {
      Task ExecuteAsync(CancellationToken cancellationToken = default);
   }

   public static class ServiceCollectionExtensions
   {
      public static IServiceCollection AddStartupTask<T>(this IServiceCollection services) where T : class, IStartupTask
         => services.AddTransient<IStartupTask, T>();
   }

   public static class StartupTaskWebHostExtensions
   {
      public static async Task RunWithTasksAsync(this IHost host, CancellationToken cancellationToken = default)
      {
         var startupTasks = host.Services.GetServices<IStartupTask>();

         foreach (var startupTask in startupTasks)
            await startupTask.ExecuteAsync(cancellationToken);

         await host.RunAsync(cancellationToken);
      }
   }

}
