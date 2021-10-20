using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow
{

   internal class StartupMigrator : IStartupTask, IDisposable
   {

      public StartupMigrator(AppSettings appSettings) =>
         _AppSettings = appSettings;
      readonly AppSettings _AppSettings;

      public async Task ExecuteAsync(CancellationToken cancellationToken = default)
      {

         try
         {

            Console.WriteLine("  Creating Context");
            using (var ctx = API.Base.dbContext.CreateNewContext(_AppSettings.ConnStr))
            {
               Console.WriteLine("  Context Created");

               Console.WriteLine("  Aplying Migration");
               await ctx.Database.MigrateAsync();
               Console.WriteLine("  Migration Applied");

            }
         }
         catch (Exception) { throw; }
      }

      public void Dispose()
      {
      }

   }

}
