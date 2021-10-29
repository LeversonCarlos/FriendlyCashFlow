using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace FriendlyCashFlow
{
   public class Program
   {
      public static async Task Main(string[] args)
      {
         await
            CreateHostBuilder(args)
            .Build()
            .RunWithTasksAsync();
      }

      public static IHostBuilder CreateHostBuilder(string[] args) =>
         Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
               webBuilder.UseStartup<Startup>();
            });

   }
}
