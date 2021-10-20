using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow
{

   internal class StartupMigrator : IStartupTask, IDisposable
   {

      public StartupMigrator(API.Base.dbContext context) =>
         _Context = context;
      readonly API.Base.dbContext _Context;

      public async Task ExecuteAsync(CancellationToken cancellationToken = default)
      {
         Console.WriteLine("  Applying Migration");
         await _Context.Database.MigrateAsync();
         Console.WriteLine("  Migration Applied");
      }

      public void Dispose()
      {
      }

   }

}
